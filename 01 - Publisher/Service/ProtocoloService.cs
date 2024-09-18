using Publisher.Models;

namespace Publisher.Service
{
    public static class ProtocoloService
    {
        private static Random random = new Random();

        public static List<Protocolo> GerarProtocolos(int quantidade)
        {
            var protocolos = new List<Protocolo>();

            foreach (var i in Enumerable.Range(1, quantidade))
            {
                var protocolo = new Protocolo
                {
                    NumeroProtocolo = Guid.NewGuid().ToString(), // Gera um número de protocolo único
                    NumeroVia = random.Next(1, 9),               // Número de via aleatório entre 1 e 9
                    Cpf = GerarCpf(),                            // Função para gerar CPF válido
                    Rg = GerarRg(),                              // Função para gerar RG aleatório
                    Nome = $"Cidadão {i}",
                    NomeMae = $"Mãe do Cidadão {i}",
                    NomePai = $"Pai do Cidadão {i}",
                    Foto = GerarFotoBase64()                     // Função para gerar uma string Base64 representando uma imagem
                };

                // Adiciona o protocolo gerado à lista
                protocolos.Add(protocolo);
            }

            // Caso 1: Dois registros com o mesmo número de protocolo
            var protocoloDuplicado = new Protocolo
            {
                NumeroProtocolo = "1234567890", // Protocolo fixo para simular duplicação
                NumeroVia = 1,
                Cpf = GerarCpf(),
                Rg = GerarRg(),
                Nome = "Cidadão Duplicado",
                NomeMae = "Mãe do Cidadão Duplicado",
                NomePai = "Pai do Cidadão Duplicado",
                Foto = GerarFotoBase64()
            };

            protocolos.Add(protocoloDuplicado);
            protocolos.Add(protocoloDuplicado);

            // Caso 2: Dois protocolos com o mesmo CPF, RG e via
            var cpfRgDuplicado = GerarCpf(); // CPF e RG fixos para simular duplicação
            var rgDuplicado = GerarRg();

            var protocoloViaDuplicada = new Protocolo
            {
                NumeroProtocolo = Guid.NewGuid().ToString(),
                NumeroVia = 2, // Mesmo número de via
                Cpf = cpfRgDuplicado,
                Rg = rgDuplicado,
                Nome = "Cidadão com RG-CPF-VIA Duplicado",
                NomeMae = "Mãe do Cidadão RG-CPF-VIA Duplicado",
                NomePai = "Pai do Cidadão RG-CPF-VIA Duplicado",
                Foto = GerarFotoBase64()
            };

            protocolos.Add(protocoloViaDuplicada);
            protocolos.Add(protocoloViaDuplicada);

            return protocolos;
        }

        private static string GerarCpf()
        {
            int[] cpf = new int[11];

            // Gera os 9 primeiros digitos do CPF
            for (int i = 0; i < 9; i++)
            {
                cpf[i] = random.Next(0, 9);
            }

            // Multiplicadores para o primeiro dígito
            var multiplicadoresPrimeiroDigito = Enumerable.Range(2, 9).Reverse().ToArray();
            int soma = 0;

            // Calcula o primeiro digito verificador
            foreach (var (numero, multiplicador) in cpf.Take(9).Zip(multiplicadoresPrimeiroDigito, (numero, multiplicador) => (numero, multiplicador)))
            {
                soma += numero * multiplicador;
            }
            int resto = soma % 11;
            cpf[9] = (resto < 2) ? 0 : 11 - resto;

            // Multiplicadores para o segundo digito
            var multiplicadoresSegundoDigito = Enumerable.Range(2, 10).Reverse().ToArray();
            soma = 0;

            // Calcula o segundo digito verificador
            foreach (var (numero, multiplicador) in cpf.Take(10).Zip(multiplicadoresSegundoDigito, (numero, multiplicador) => (numero, multiplicador)))
            {
                soma += numero * multiplicador;
            }
            resto = soma % 11;
            cpf[10] = (resto < 2) ? 0 : 11 - resto;

            return string.Join("", cpf);
        }

        private static string GerarRg()
        {
            return new Random().Next(10000000, 99999999).ToString();
        }

        private static string GerarFotoBase64()
        {
            // Simulação de uma foto codificada em Base64
            return Convert.ToBase64String(new byte[] { 255, 216, 255, 224 });
        }
    }
}