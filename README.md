# random-user-generator-api

API RESTful desenvolvida em ASP.NET Core que gera e gerencia usuários. A API consome dados de uma API externa (Random User API) e os persiste em um banco de dados PostgreSQL usando Entity Framework Core. 

- A transferência de dados é realizada por objetos DTO para controlar a visualização e exposição de dados tanto para o usuário quanto para classes internas.

- O projeto também inclui uma interface web simples em HTML/CSS/JavaScript para demonstração dos endpoints.

- A documentação Swagger é exibida quando a aplicação é iniciada.


## ⚙️ Arquitetura do Projeto

O projeto segue uma arquitetura em camadas, no padrão Controller/Service/Repository, promovendo a separação de responsabilidades e facilitando a manutenção:

- Controllers: Lidam com as requisições HTTP e roteamento.

- Services (Camada de Negócio): Contêm a lógica principal, como a chamada à API externa (https://randomuser.me/api/) e a validação de dados.

- Repositories (Camada de Dados): Abstraem a interação com o banco de dados (CRUD) usando Entity Framework Core.

- wwwroot: Contém os arquivos estáticos da interface web (HTML, CSS, JavaScript).


## 🛠️ Pré-requisitos

Para rodar o projeto localmente, você precisará ter instalado:

1. [.NET SDK (Versão 7.0 ou superior)](https://dotnet.microsoft.com/pt-br/download)

2. [PostgreSQL Server (Versão 12 ou superior recomendada)](https://www.postgresql.org/download/)


## 💻 Configuração e Instalação

Siga estes passos para configurar e executar a API.

1. Clonar o Repositório

```Bash

git clone https://github.com/ytosati/random-user-generator-api.git
cd random-user-generator-api
```

2. Configurar o Banco de Dados

    A API utiliza o PostgreSQL. Você precisa configurar as credenciais de acesso.

    - Criar o Banco de Dados
   
      Crie um banco de dados vazio no seu servidor PostgreSQL. O nome padrão utilizado no projeto é random_user_generator_db.

      A tabela será criada automaticamente pelo EF Core

    - Atualizar a Connection String

      Edite o arquivo appsettings.json e atualize a string de conexão DefaultConnection com suas credenciais do PostgreSQL.

#### ⚠️ IMPORTANTE: 

Certifique-se de que o Username e Password correspondem ao seu banco de dados local.

```JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=random_user_generator_db;Username=SEU_USUARIO_POSTGRES;Password=SUA_SENHA_POSTGRES"
  },
}
```

3. Instalar Dependências e Rodar Migrações

    O Entity Framework Core aplicará as migrações automaticamente na inicialização, criando as tabelas necessárias.

- Restaurar Pacotes 
```Bash
  dotnet restore
  ```

- Executar o Projeto

```Bash
dotnet run
```


## 🚀 Uso e Endpoints

Após a execução, a API estará acessível nas seguintes URLs (padrão do launchSettings.json):

- HTTPS: https://localhost:7068

- HTTP: http://localhost:5229

Interface Web (Frontend)
Execute o arquivo index.html, dentro da pasta wwwroot:

🔗 Acessar Interface: https://localhost:7068/

A interface permite testar os seguintes endpoints.

Endpoints da API
Você pode testar os endpoints diretamente usando ferramentas como Swagger UI ou Postman/Insomnia.

🔗 Documentação (Swagger UI): https://localhost:7068/swagger

| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| **`POST`** | `/api/Users/generate` | Gera um novo usuário aleatório através de uma API externa, salva os dados no banco de dados e retorna o registro criado (incluindo a senha). |
| **`GET`** | `/api/Users` | Lista todos os usuários cadastrados no banco de dados. |
| **`PATCH`** | `/api/Users/{id}` | Atualiza parcialmente os dados de um usuário pelo seu `Id` (nome, telefone ou senha). Requer a senha atual para alteração de senha. |


### Como utilizar:

Ao preparar o ambiente, o banco estará vazio. Fazer requisições Post irá popular o banco com as informações consumidas pela API externa.

Com o banco populado, é possível buscar o relatório com o método Get ou alterar dados com o método Patch, que já salva a alteração no banco.

Optei por trabalhar com uma simples lógica de negócio onde o campo Senha só aparece para o usuário no momento em que o método Post é chamado. 

Após esse momento, a senha fica disponível no banco mas não pode ser visualizada pelo relatório geral (método get)

Finalmente, para que altere o campo Senha por meio do método Patch, o usuário deve saber a senha atual, e confirmar a nova senha, para evitar mudanças de senha por erros de digitação.
