using Consumer.Models;
using Serilog;


namespace Consumer.Service
{
    public class ProtocoloService
    {
        // Método para validar o protocolo usando Entity Framework
        public static bool ValidarProtocolo(Protocolo protocolo, ProtocoloContext dbContext)
        {
            if (string.IsNullOrEmpty(protocolo.NumeroProtocolo) ||
                string.IsNullOrEmpty(protocolo.Cpf) ||
                string.IsNullOrEmpty(protocolo.Rg) ||
                string.IsNullOrEmpty(protocolo.Nome) ||
                protocolo.Foto == null)
            {
                return false;
            }

            // Valida se o protocolo já existe
            var protocoloExistente = dbContext.Protocolos.Any(p => p.NumeroProtocolo == protocolo.NumeroProtocolo);
            if (protocoloExistente)
            {
                Log.Warning("Protocolo duplicado encontrado: {0}", protocolo.NumeroProtocolo);
                return false;
            }

            // Valida se o CPF/RG com a mesma via já existe
            var viaExistente = dbContext.Protocolos.Any(p => p.Cpf == protocolo.Cpf && p.Rg == protocolo.Rg && p.NumeroVia == protocolo.NumeroVia);
            if (viaExistente)
            {
                Log.Warning("Duplicidade de CPF/RG com o mesmo número de via: {0}, {1}, Via: {2}", protocolo.Cpf, protocolo.Rg, protocolo.NumeroVia);
                return false;
            }

            return true;
        }

        // Método para salvar o protocolo no banco de dados usando Entity Framework
        public static void SalvarProtocolo(Protocolo protocolo, ProtocoloContext dbContext)
        {
            dbContext.Protocolos.Add(protocolo);
            dbContext.SaveChanges();
            Log.Information("Protocolo salvo com sucesso: {0}", protocolo.NumeroProtocolo);
        }
    }
}