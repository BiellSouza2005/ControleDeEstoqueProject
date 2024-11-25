using ControleDeEstoqueAPI.Data.DTOs.User;
using ControleDeEstoqueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("VerUsuarioPor/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return NotFound("Usuário não encontrado ou não está inativo.");

        return Ok(user);
    }

    [HttpGet("VerTodosUsuarios")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllInactiveUsersAsync();
        if (!users.Any())
            return NotFound("Nenhum usuário inativo encontrado.");

        return Ok(users);
    }

    [HttpPost("AdicionarUsuario")]
    public async Task<IActionResult> AddUser([FromBody] UserRegistrationDTO userDto, [FromHeader(Name = "User-Inclusion")] string userInclusion)
    {
        var user = await _userRepository.AddUserAsync(userDto, userInclusion);
        if (user == null)
            return Conflict("O usuário já existe.");

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
    }

    [HttpPut("AtualizarUsuario/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDto, [FromHeader(Name = "User-Inclusion")] string userChange)
    {
        var result = await _userRepository.UpdateUserAsync(id, userDto, userChange);
        if (!result) return NotFound("Usuário não encontrado.");

        return Ok("Usuário atualizado com sucesso.");
    }

    [HttpDelete("DeletarUsuario/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userRepository.DeleteUserAsync(id);
        if (!result) return NotFound("Usuário não encontrado.");

        return NoContent();
    }

    [HttpDelete("DesativarUsuario/{id}")]
    public async Task<IActionResult> DisableUser(int id, [FromHeader(Name = "User-Inclusion")] string userChange)
    {
        var result = await _userRepository.DisableUserAsync(id, userChange);
        if (!result) return NotFound("Usuário não encontrado.");

        return NoContent();
    }

    [HttpPost("LoginUsuario")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        var user = await _userRepository.LoginAsync(loginDto);
        if (user == null)
            return BadRequest("Usuário ou senha incorretos.");

        return Ok(new { mensagem = "Login realizado com sucesso!", token = Guid.NewGuid(), user });
    }
}
