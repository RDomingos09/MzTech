using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Publisher.Service
{
    internal class PublisherService
    {
        private const string QUEUE_NAME = "protocolos_fila";
        private const string DEAD_LETTER_QUEUE_NAME = "protocolos_fila_dlq";

        public static void Main(string[] args)
        {
            // Gerar 10 protocolos mocados
            var protocolos = ProtocoloService.GerarProtocolos(10);

            // Configuração de conexão com o RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declara a fila principal com DLQ configurada
                var arguments = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", "" },
                    { "x-dead-letter-routing-key", DEAD_LETTER_QUEUE_NAME }
                };

                channel.QueueDeclare(queue: QUEUE_NAME,
                                     durable: true,
                                     exclusive: false,
                                     arguments: arguments);

                // Declara a DLQ
                channel.QueueDeclare(queue: DEAD_LETTER_QUEUE_NAME,
                                     durable: true,
                                     exclusive: false,
                                     arguments: null);

                foreach (var protocolo in protocolos)
                {
                    var mensagem = JsonConvert.SerializeObject(protocolo);
                    var body = Encoding.UTF8.GetBytes(mensagem);

                    channel.BasicPublish(exchange: "",
                                         routingKey: QUEUE_NAME,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [x] Protocolo {protocolo.NumeroProtocolo} enviado para a fila.");
                }
            }
        }
    }
}