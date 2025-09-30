using Microsoft.EntityFrameworkCore;
using random_user_generator_api.Data;
using random_user_generator_api.Repositories;
using random_user_generator_api.Services;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.OpenApi.Models;
using System.Reflection;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
});

//Configura��es de documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Generator and Management API",
        Version = "v1",
        Description = "API para gerar usu�rios aleat�rios (via API externa) e realizar opera��es de gerenciamento (GET, PATCH).",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

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