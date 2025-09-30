using Microsoft.AspNetCore.Mvc;
using random_user_generator_api.Services;
using random_user_generator_api.DTOs;
using random_user_generator_api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

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
            //return CreatedAtAction(nameof(GenerateUser), new { id = newUser.Id }, newUser);
            var responseDto = new UserCreationResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Password = newUser.Password,
                PhoneNumber = newUser.PhoneNumber,
                DateOfBirth = newUser.DateOfBirth,
                StreetName = newUser.StreetName,
                StreetNumber = newUser.StreetNumber,
                City = newUser.City,
                State = newUser.State,
                Country = newUser.Country
            };

            return StatusCode(201, responseDto);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(503, new { message = "Serviço externo indisponível ou erro de rede", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<UserResponseDto>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<UserResponseDto>> PatchUser([FromRoute] int id, [FromBody] UserRequestDto requestDto)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, requestDto);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            //404 Not Found - Usuário não encontrado no banco
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            //400 Bad Request - Erro de validação (Nova Senha e Confirmação não batem, ou campos de senha incompletos)
            return BadRequest(new { message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            //401 Unauthorized - Senha atual incorreta
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            //500 Internal Server Error
            return StatusCode(500, new { message = "Erro interno ao atualizar usuário.", details = ex.Message });
        }
    }
}