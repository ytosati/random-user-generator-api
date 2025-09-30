using System;
using Microsoft.EntityFrameworkCore;
using random_user_generator_api.Data;
using random_user_generator_api.Entities;

namespace random_user_generator_api.Repositories
{
    //Implementação da interface IUserRepository
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            //Busca todos os registros da tabela Users
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            //salva as alterações se houver modificações.
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

}
