using System;

namespace random_user_generator_api.DTOs
{
    //DTO usado para retornar a lista de usu�rios e omitir a senha e Uuid para o cliente
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}