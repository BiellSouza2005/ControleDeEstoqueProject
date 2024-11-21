using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Brand;
using ControleDeEstoqueAPI.Data.DTOs.Login;
using ControleDeEstoqueAPI.Data.DTOs.Product;
using ControleDeEstoqueAPI.Data.DTOs.ProductType;
using ControleDeEstoqueAPI.Data.DTOs.User;
using ControleDeEstoqueAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("VerUsuarioPor/{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        var userDto = new UserDTO
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };

        return Ok(userDto);
    }

    [HttpGet("VerTodosUsuarios")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDTO
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost("AdicionarUsuario")]
    public async Task<IActionResult> AddUser(UserRegistrationDTO userDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
        if (existingUser != null)
        {
            return NotFound("O usuário já existe.");
        }

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
    }

    [HttpPut("AtualizarUsuario/{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDTO userDto)
    {
        if (id != userDto.UserId)
        {
            return BadRequest("ID do usuário não corresponde.");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.Password = userDto.Password;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("DeletarUsuario/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("LoginUsuario")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        // Verifica se existe um usuário com o email fornecido
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // Se o usuário não for encontrado ou a senha estiver incorreta
        if (user == null || user.Password != loginDto.Password)
        {
            return BadRequest("Usuário ou senha incorretos.");
        }

        // Se estiver tudo certo, retornar o token de acesso e uma mensagem de sucesso
        return Ok(new { mensagem = "Login realizado com sucesso!", token = Guid.NewGuid(), user });
    }
}
