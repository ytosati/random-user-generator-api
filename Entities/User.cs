using System;
using System.ComponentModel.DataAnnotations;

namespace random_user_generator_api.Entities
{
    public class User
    {
        //Chave primária da tabela
        public int Id { get; set; }

        //Identificador único disponibilizado pela API
        [Required]
        public string Uuid { get; set; }

        //Name
        [Required]
        public string Name { get; set; }

        //Email
        [Required]
        public string Email { get; set; }

        //Phone
        public string PhoneNumber { get; set; }

        //Birthday
        public DateTime DateOfBirth { get; set; }

        //Address
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        //Password
        public string Password { get; set; }

        //Metadados e auditoria
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}