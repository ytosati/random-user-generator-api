using random_user_generator_api.Entities;
using random_user_generator_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace random_user_generator_api.Services
{
    //Declaração dos métodos da API
    public interface IUserService
    {
        //Criar / salvar usuário no db
        Task<User> FetchAndSaveRandomUserAsync();

        //Listar usuários salvos no db
        Task<List<UserResponseDto>> GetAllUsersAsync();

        //Atualizar informações
        Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto requestDto);
    }
}