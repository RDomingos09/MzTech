Projeto de Recep��o de Protocolos
Este projeto consiste em uma solu��o para recep��o e processamento de protocolos de solicita��o de emiss�o de documentos de identidade (RG). A solu��o inclui os seguintes componentes:

Publisher: Respons�vel por publicar protocolos (mensagens) na fila do RabbitMQ.
Consumer: Consome as mensagens da fila, valida e persiste os protocolos no banco de dados.
API: Disponibiliza uma interface REST para consulta dos protocolos.

Tecnologias Utilizadas
.NET 7
RabbitMQ (via Docker)
Entity Framework Core (EF Core)
SQL Server (via Docker)
Polly para retries
Swagger para documenta��o da API
Serilog para logging
Requisitos
.NET 7 SDK instalado.
Docker e Docker Compose configurados e em execu��o.

Configura��o Inicial
1. Clone o reposit�rio:
git clone https://github.com/seu-repositorio/projeto-de-recepcao-de-protocolos.git
cd projeto-de-recepcao-de-protocolos

2. Configurando o RabbitMQ (via Docker)
Inicie o RabbitMQ com o Docker Compose:
docker-compose up -d

3. Migra��es do Banco de Dados
Caso seja necess�rio, voc� pode criar as migra��es e atualizar o banco de dados utilizando os comandos do Entity Framework Core:

cd Consumer
dotnet ef migrations add InitialCreate

Atualizar o banco de dados:
dotnet ef database update

Voc� pode acessar a interface do RabbitMQ em: http://localhost:15672 (usu�rio: guest, senha: guest).

Execu��o do Projeto

4. Publicar Mensagens (Publisher)
Navegue at� o diret�rio do Publisher e execute o projeto:
cd Publisher
dotnet run

Isso enviar� 10 protocolos mockados para a fila RabbitMQ.

5. Consumir Mensagens (Consumer)
Navegue at� o diret�rio do Consumer e execute o projeto:
cd Consumer
dotnet run

O Consumer vai consumir as mensagens, validar e salv�-las no banco de dados.

6. Iniciar a API
Navegue at� o diret�rio da API e execute o projeto:
cd Api
dotnet run

A API estar� dispon�vel em http://localhost:5000

Swagger em http://localhost:5238/swagger/index.html

Endpoints da API

Autentica��o com Token JWT
Antes de utilizar os endpoints da API, voc� deve se autenticar e obter um token JWT.
Obtenha o token JWT via o endpoint /auth/token

POST /auth/token
{
  "username": "admin",
  "password": "password"
}

Logs
Os logs do Consumer s�o gerados no arquivo consumer.log no diret�rio do projeto. Logs tamb�m s�o mostrados no console e incluem:

Erros de processamento.
Mensagens de sucesso ao salvar protocolos no banco.
Tentativas de retry no caso de falhas (usando Polly).