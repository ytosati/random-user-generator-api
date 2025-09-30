using System;
using System.Collections.Generic;

//Utilizando arquitetura de DTO para encapsular a passagem de dados da entidade para o serviço e controle, e vice-versa
namespace random_user_generator_api.DTOs
{
    //Classe que recebe a lista de usuários no array 'results', da forma como vem na API
    public class RandomUserApiResponse
    {
        public List<ApiUserResult> Results { get; set; } = new List<ApiUserResult>();
    }

    //Classe que recebe o usuário dentro da lista 'results'
    public class ApiUserResult
    {
        public ApiUserName Name { get; set; }
        public string Email { get; set; }
        public ApiUserLogin Login { get; set; }
        public ApiUserDateOfBirth Dob { get; set; }
        public ApiUserLocation Location { get; set; }
        public string Phone { get; set; }
    }

    //Mapeamento para o campo name
    public class ApiUserName
    {
        public string First { get; set; }
        public string Last { get; set; }
    }

    //Mapeamento para o campo login
    public class ApiUserLogin
    {
        public string Uuid { get; set; }
        public string Password { get; set; }
    }

    //Mapeamento para o campo dob (date of birth)
    public class ApiUserDateOfBirth
    {
        public DateTime? Date { get; set; }
    }

    //Mapeamento para o campo location
    public class ApiUserLocation
    {
        public ApiUserStreet Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    //Mapeamento para o campo street dentro de location
    public class ApiUserStreet
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}