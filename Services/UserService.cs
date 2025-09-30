using random_user_generator_api.Entities;
using random_user_generator_api.Repositories;
using random_user_generator_api.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace random_user_generator_api.Services
{
    //Implementação dos métodos da API
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;

        public UserService(IUserRepository userRepository, HttpClient httpClient)
        {
            _userRepository = userRepository;
            _httpClient = httpClient;
        }

        //Criar / salvar usuário no db
        public async Task<User> FetchAndSaveRandomUserAsync()
        {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/");

            //Lançamento de exceções
            response.EnsureSuccessStatusCode();

            //Deserialize do json para os DTOs de mapeamento
            var jsonString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<RandomUserApiResponse>(jsonString);

            //Garante que pelo menos um usuário foi retornado
            if (apiResponse?.Results == null || !apiResponse.Results.Any())
            {
                throw new ApplicationException("A API externa não retornou nenhum usuário.");
            }

            var apiUserResult = apiResponse.Results.First();

            //Mapeamento dos DTOs para a entidade User
            var userToSave = MapApiResultToUser(apiUserResult);

            //Chama o repositório
            await _userRepository.AddAsync(userToSave);

            return userToSave;
        }

        //Método auxiliar para mapeamento
        private User MapApiResultToUser(ApiUserResult apiUser)
        {
            return new User
            {
                Uuid = apiUser.Login.Uuid,

                FirstName = apiUser.Name.First,
                LastName = apiUser.Name.Last,

                Email = apiUser.Email,
                PhoneNumber = apiUser.Phone,

                DateOfBirth = apiUser.Dob.Date ?? DateTime.MinValue,

                StreetName = apiUser.Location.Street.Name,
                StreetNumber = apiUser.Location.Street.Number,
                City = apiUser.Location.City,
                State = apiUser.Location.State,
                Country = apiUser.Location.Country,

                Password = apiUser.Login.Password,
            };
        }

        //Listar usuários salvos no db
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        //Atualizar informações
        public async Task<UserResponseDto> UpdateUserAsync(int id, UserResponseDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}