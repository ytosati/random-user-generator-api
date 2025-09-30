using Microsoft.EntityFrameworkCore;
using random_user_generator_api.Entities;

//Utilizando EF Core para ter acesso à classe User
namespace random_user_generator_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}