using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Consumer.Models;
using Consumer.Service;
using Polly;

public class Program
{
    private const string QueueName = "protocolos_fila";
    private const string DeadLetterQueueName = "protocolos_fila_dlq";
    private const int MaxRetryAttempts = 3;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(5); // Tempo de espera entre tentativas

    public static void Main(string[] args)
    {
        // Configura o Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/consumer.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Iniciando o consumer...");

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declara a fila principal com DLQ configurada
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", DeadLetterQueueName }
            };

            channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, arguments: arguments);

            // Declara a DLQ
            channel.QueueDeclare(queue: DeadLetterQueueName, durable: true, exclusive: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Log.Information("Mensagem recebida: {0}", message);

                // Define a política de retry com Polly
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(MaxRetryAttempts, attempt => RetryDelay, (exception, timeSpan, retryCount, context) =>
                    {
                        Log.Warning("Tentativa {0}/{1} falhou. Tentando novamente em {2} segundos. Erro: {3}",
                            retryCount, MaxRetryAttempts, RetryDelay.Seconds, exception.Message);
                    });

                try
                {
                    retryPolicy.Execute(() =>
                    {
                        var protocolo = JsonConvert.DeserializeObject<Protocolo>(message);
                        var optionsBuilder = new DbContextOptionsBuilder<ProtocoloContext>();
                        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ProtocoloDB;User Id=sa;Password=NovaSenhaForte123!;Encrypt=False");

                        using (var dbContext = new ProtocoloContext(optionsBuilder.Options))
                        {
                            if (ProtocoloService.ValidarProtocolo(protocolo, dbContext))
                            {
                                ProtocoloService.SalvarProtocolo(protocolo, dbContext);
                                channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                                Log.Information("Protocolo salvo com sucesso.");
                            }
                            else
                            {
                                Log.Warning("Protocolo inválido: {0}", message);
                                channel.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    // Após todas as tentativas, envia para DLQ sem reenfileirar na fila principal
                    Log.Error("Erro após {0} tentativas. Falha ao processar mensagem: {1}. Erro: {2}", MaxRetryAttempts, message, ex.Message);
                    channel.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);
                }
            };

            channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            Log.Information("Aguardando mensagens...");
            Console.ReadLine();
        }
    }
}