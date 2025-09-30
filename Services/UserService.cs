using random_user_generator_api.Entities;
using random_user_generator_api.Repositories;
using random_user_generator_api.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

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
            var response = await _httpClient.GetAsync("https://randomuser.me/api/");

            //Lan�amento de exce��es
            response.EnsureSuccessStatusCode();

            //Deserialize do json para os DTOs de mapeamento
            var jsonString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<RandomUserApiResponse>(jsonString);

            //Garante que pelo menos um usu�rio foi retornado
            if (apiResponse?.Results == null || !apiResponse.Results.Any())
            {
                throw new ApplicationException("A API externa n�o retornou nenhum usu�rio.");
            }

            var apiUserResult = apiResponse.Results.First();

            //Mapeamento dos DTOs para a entidade User
            var userToSave = MapApiResultToUser(apiUserResult);

            //Chama o reposit�rio
            await _userRepository.AddAsync(userToSave);

            return userToSave;
        }

        //M�todo auxiliar para mapeamento
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

        //Listar usu�rios salvos no db
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        //Atualizar informa��es
        public async Task<UserResponseDto> UpdateUserAsync(int id, UserResponseDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}