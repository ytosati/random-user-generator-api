using random_user_generator_api.Entities;
using random_user_generator_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace random_user_generator_api.Services
{
    //Declara��o dos m�todos da API
    public interface IUserService
    {
        //Criar / salvar usu�rio no db
        Task<User> FetchAndSaveRandomUserAsync();

        //Listar usu�rios salvos no db
        Task<List<UserResponseDto>> GetAllUsersAsync();

        //Atualizar informa��es
        Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto requestDto);
    }
}