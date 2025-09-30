using random_user_generator_api.Entities;
using random_user_generator_api.Repositories;
using random_user_generator_api.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace random_user_generator_api.Services
{
    //Implementa��o dos m�todos da API
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;

        public UserService(IUserRepository userRepository, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _httpClient = httpClient;
        }

        //Criar / salvar usu�rio no db
        public async Task<User> FetchAndSaveRandomUserAsync()
        {
            throw new NotImplementedException();
        }

        //Listar usu�rios salvos no db
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        //Atualizar informa��es
        public async Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}