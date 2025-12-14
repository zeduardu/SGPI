using SGPI.Application.DTOs;
using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<Usuario> CreateUserAsync(CreateUserRequest request, string password);
        Task UpdateUserAsync(string id, Usuario user);
        Task DeleteUserAsync(string id);
        Task AssignRoleAsync(string userId, string role);
    }
}
