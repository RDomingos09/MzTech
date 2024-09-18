Projeto de Recepção de Protocolos

Este projeto consiste em uma solução para recepção e processamento de protocolos de solicitação de emissão de documentos de identidade (RG). A solução inclui os seguintes componentes:

Publisher: Responsável por publicar protocolos (mensagens) na fila do RabbitMQ.
Consumer: Consome as mensagens da fila, valida e persiste os protocolos no banco de dados.
API: Disponibiliza uma interface REST para consulta dos protocolos.

Tecnologias Utilizadas

.NET 7
RabbitMQ (via Docker)
Entity Framework Core (EF Core)
SQL Server (via Docker)
Polly para retries
Swagger para documentação da API
Serilog para logging
Requisitos
.NET 7 SDK instalado.
Docker e Docker Compose configurados e em execução.

Configuração Inicial

1. Clone o repositório:
git clone https://github.com/RDomingos09/MzTech.git

cd protocolo

2. Configurando o RabbitMQ (via Docker)
Inicie o RabbitMQ com o Docker Compose:

docker-compose up -d

4. Migrações do Banco de Dados
Caso seja necessário, você pode criar as migrações e atualizar o banco de dados utilizando os comandos do Entity Framework Core:

cd Consumer
dotnet ef migrations add InitialCreate

Atualizar o banco de dados:

dotnet ef database update

Você pode acessar a interface do RabbitMQ em: http://localhost:15672 (usuário: guest, senha: guest).

Execução do Projeto

4. Publicar Mensagens (Publisher)
Navegue até o diretório do Publisher e execute o projeto:

cd Publisher
dotnet run

Isso enviará 10 protocolos mockados para a fila RabbitMQ.

5. Consumir Mensagens (Consumer)
Navegue até o diretório do Consumer e execute o projeto:

cd Consumer
dotnet run

O Consumer vai consumir as mensagens, validar e salvá-las no banco de dados.

6. Iniciar a API
Navegue até o diretório da API e execute o projeto:

cd Api
dotnet run

A API estará disponível em http://localhost:5000

Swagger em http://localhost:5238/swagger/index.html

Endpoints da API

Autenticação com Token JWT
Antes de utilizar os endpoints da API, você deve se autenticar e obter um token JWT.
Obtenha o token JWT via o endpoint /auth/token

POST /auth/token
{
  "username": "admin",
  "password": "password"
}

Logs
Os logs do Consumer são gerados no arquivo consumer.log no diretório do projeto. Logs também são mostrados no console e incluem:

Erros de processamento.
Mensagens de sucesso ao salvar protocolos no banco.
Tentativas de retry no caso de falhas (usando Polly).
