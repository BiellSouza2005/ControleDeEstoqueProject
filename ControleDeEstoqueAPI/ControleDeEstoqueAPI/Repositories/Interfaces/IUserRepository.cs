using ControleDeEstoqueAPI.Data.DTOs.User;
using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Repositories
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllInactiveUsersAsync();
        Task<User?> AddUserAsync(UserRegistrationDTO userDto, string userInclusion);
        Task<bool> UpdateUserAsync(int id, UserDTO userDto, string userChange);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> DisableUserAsync(int id, string userChange);
        Task<User?> LoginAsync(UserLoginDTO loginDto);
    }
}
