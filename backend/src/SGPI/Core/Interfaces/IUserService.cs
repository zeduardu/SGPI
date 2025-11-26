using SGPI.Application.DTOs;
using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<Usuario> CreateUserAsync(Usuario user, string password);
        Task UpdateUserAsync(string id, Usuario user);
        Task DeleteUserAsync(string id);
        Task AssignRoleAsync(string userId, string role);
    }
}
