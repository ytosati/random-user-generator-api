using System;
using System.ComponentModel.DataAnnotations;

namespace random_user_generator_api.DTOs
{
    //DTO usado para retornar o corpo de resposta do método post, contendo a senha que é omitida no UserResponseDto padrão.
    public class UserCreationResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
