random-user-generator-api

Log de alterações:
<header>V0</header>

1. Configuração e Infraestrutura (Program.cs e appsettings.json)
Configurado o framework ASP.NET Core para rodar a aplicação e conectar ao banco de dados.
Foi configurada a conexão com o PostgreSQL via Entity Framework Core (EF Core), utilizando a connection string definida no appsettings.json. O Program.cs também estabeleceu a Injeção de Dependência (DI) para todas as camadas (Services, Repositories) e o HttpClient para consumo da API externa.

2. Camada de Persistência e Acesso a Dados (Entities, Data, Repositories)
Criado o modelo de dados e a ponte entre a aplicação e o PostgreSQL.

Entities/User.cs: Define a estrutura da tabela no banco de dados, mapeando os campos de interesse da API externa (name, email, phone, birthday, address, password, uuid).

Data/ApplicationDbContext.cs: Classe central do EF Core que traduz as entidades C# em comandos SQL.

Repositories (IUserRepository.cs e UserRepository.cs): Cria a abstração do acesso a dados com todos os métodos CRUD necessários (Add, Get All, Get By Id, Update), isolando o serviço da tecnologia de banco de dados.

3. Camada de Transferência de Dados (DTOs)
O objetivo é Controlar a entrada e saída de dados, formatando e ocultando informações internas.

UserResponseDto.cs: Define o formato de saída para o cliente (Web/Front-end), omitindo dados sensíveis (como a senha e o Uuid).

RandomUserApiModels.cs: Define o formato de entrada para o consumo da API externa, espelhando exatamente a estrutura JSON aninhada da API randomuser.me.

4. Camada de Lógica de Negócio (Services)
Criada a base que trabalhará a lógica de negócio principal da API.

A interface IUserService.cs foi criada para declarar os três métodos principais da API: Criar e Salvar (consumo da API), Listar Usuários (relatório) e Atualizar Usuário. A classe UserService.cs está pronta para ser implementada, orquestrando o HttpClient e o UserRepository.

<header>V1</header>

1. Resolvidas várias dependências de pacote 
já possuia o visual studio na versão 9.0 e ao baixar alguns pacotes, ocorreram conflitos, que já foram resolvidos.

2. implementado o primeiro método FetchAndSaveRandomUserAsync
o método consome a api e recebe seus dados um objeto User que é salvo no banco.
já foi testado, e está funcional, mas precisará passar por tratamento no output dos dados.

<header>V2</header>

1. Implementados métodos get e patch
get retorna uma lista (omitindo senha e dados de auditoria, que ficam disponíveis no banco de dados)
patch envia um corpo de requisição json informando qual campo deve ser atualizado.
em caso de alteração de senha, será necessário preencher o campo senha atual (para garantir que só quem saiba a senha possa alterará-la) e confirmação de nova senha (para evitar que uma nova senha seja cadastrada com erros de digitação)

2. Tratados dados de saída no corpo de resposta das requisições
método post passa a retornar a senha (já que o usuário está sendo criado, este será o primeiro contato do usuário com a senha, e ele precisa saber a senha caso queira alterar posteriormente). Dados de auditoria e Uuid omitidos e disponíveis apenas no DB.
método get omite senha e dados de auditoria, para proteger informações sensíveis a quem acessar o relatório.