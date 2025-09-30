using Microsoft.AspNetCore.Mvc;
using random_user_generator_api.Services;
using random_user_generator_api.Entities;
using System.Threading.Tasks;

//Definição de rota e responsabilidade da classe
[ApiController]
[Route("api/[controller]")] 
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    //Endpoint 1
    [HttpPost("generate")]
    public async Task<ActionResult<User>> GenerateUser()
    {
        try
        {
            var newUser = await _userService.FetchAndSaveRandomUserAsync();
            return CreatedAtAction(nameof(GenerateUser), new { id = newUser.Id }, newUser);
        }
        catch(ApplicationException ex)
        {
            return StatusCode(500, new { message = "Erro na API externa", details = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(503, new { message = "Serviço externo indisponível ou erro de rede", details = ex.Message });
        }
    }
}