using random_user_generator_api.Entities;

namespace random_user_generator_api.Repositories
{
    //Definição dos métodos CRUD
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task UpdateAsync(User user);
    }
}