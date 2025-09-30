using random_user_generator_api.Entities;
using random_user_generator_api.Repositories;
using random_user_generator_api.DTOs;
using random_user_generator_api.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;

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
            DateTime dateOfBirthValue = apiUser.Dob.Date.Value.Date;
            dateOfBirthValue = DateTime.SpecifyKind(dateOfBirthValue, DateTimeKind.Unspecified);

            return new User
            {
                Uuid = apiUser.Login.Uuid,

                Name = $"{apiUser.Name.First}{" "}{apiUser.Name.Last}",

                Email = apiUser.Email,
                PhoneNumber = apiUser.Phone,

                DateOfBirth = dateOfBirthValue,

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
            var users = await _userRepository.GetAllUsersAsync();

            var responseList = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                StreetName = u.StreetName,
                StreetNumber = u.StreetNumber,
                City = u.City,
                State = u.State,
                Country = u.Country
            }).ToList();

            return responseList;
        }

        //Atualizar informações
        public async Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto requestDto)
        {
            //Busca o usuário pelo ID
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");
            }

            /*Lógica de Troca de Senha. Optei por só permitir que o usuário mude sua senha caso mande na requisição
             sua senha atual, e confirme a senha que quer alterar.*/
            var isPasswordChangeRequested = !string.IsNullOrWhiteSpace(requestDto.NovaSenha) ||
                                            !string.IsNullOrWhiteSpace(requestDto.ConfirmaNovaSenha) ||
                                            !string.IsNullOrWhiteSpace(requestDto.SenhaAtual);

            if (isPasswordChangeRequested)
            {
                if (requestDto.SenhaAtual != user.Password)
                {
                    throw new ApplicationException("A Senha Atual fornecida está incorreta.");
                }

                if (string.IsNullOrWhiteSpace(requestDto.NovaSenha) || string.IsNullOrWhiteSpace(requestDto.ConfirmaNovaSenha))
                {
                    throw new ArgumentException("Nova Senha e Confirmação da Nova Senha são obrigatórias para troca de senha.");
                }

                if (requestDto.NovaSenha != requestDto.ConfirmaNovaSenha)
                {
                    throw new ArgumentException("Nova Senha e Confirmação da Nova Senha não coincidem.");
                }

                user.Password = requestDto.NovaSenha;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Name))
            {
                user.Name = requestDto.Name;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                user.Email = requestDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.PhoneNumber))
            {
                user.PhoneNumber = requestDto.PhoneNumber;
            }

            if (requestDto.DateOfBirth.HasValue)
            {
                user.DateOfBirth = DateTime.SpecifyKind(requestDto.DateOfBirth.Value.Date, DateTimeKind.Unspecified);
            }

            if (!string.IsNullOrWhiteSpace(requestDto.StreetName))
            {
                user.StreetName = requestDto.StreetName;
            }

            if (requestDto.StreetNumber.HasValue)
            {
                user.StreetNumber = requestDto.StreetNumber.Value;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.City))
            {
                user.City = requestDto.City;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.State))
            {
                user.State = requestDto.State;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Country))
            {
                user.Country = requestDto.Country;
            }

            //Salva as alterações
            await _userRepository.UpdateAsync(user);

            //Retorna o corpo de resposta
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                StreetName = user.StreetName,
                StreetNumber = user.StreetNumber,
                City = user.City,
                State = user.State,
                Country = user.Country
            };
        }
    }
}