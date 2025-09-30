using Microsoft.EntityFrameworkCore;
using random_user_generator_api.Data;
using random_user_generator_api.Repositories;
using random_user_generator_api.Services;

var builder = WebApplication.CreateBuilder(args);

//Configura��o do EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);

//Inje��o de depend�ncias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<IUserService, UserService>();

//Servi�os padr�o
builder.Services.AddControllers(); 

//Configura��es de documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Aplica as migra��es automaticamente na inicializa��o
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();