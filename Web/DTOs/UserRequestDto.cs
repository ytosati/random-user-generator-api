using System;
using System.ComponentModel.DataAnnotations;

namespace random_user_generator_api.DTOs
{
    public class UserRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? StreetName { get; set; }
        public int? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public string? SenhaAtual { get; set; }
        public string? NovaSenha { get; set; }
        public string? ConfirmaNovaSenha { get; set; }
    }
}
