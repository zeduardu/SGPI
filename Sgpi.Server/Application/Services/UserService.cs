using SGPI.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
  public class UserService : IUserService
  {
    private readonly UserManager<Usuario> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
      var users = await _userManager.Users.ToListAsync();
      var userDtos = new List<UserDto>();

      foreach (var user in users)
      {
        var roles = await _userManager.GetRolesAsync(user);
        userDtos.Add(new UserDto
        {
          Id = user.Id,
          UserName = user.UserName ?? string.Empty,
          Email = user.Email ?? string.Empty,
          NomeCompleto = user.NomeCompleto,
          Roles = roles.ToList()
        });
      }

      return userDtos;
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return null;

      var roles = await _userManager.GetRolesAsync(user);
      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName ?? string.Empty,
        Email = user.Email ?? string.Empty,
        NomeCompleto = user.NomeCompleto,
        Roles = roles.ToList()
      };
    }

    public async Task<List<string>> GetRolesAsync(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return new List<string>();

      var roles = await _userManager.GetRolesAsync(user);
      return roles.ToList();
    }

    public async Task<Usuario> CreateUserAsync(CreateUserRequest request, string password)
    {
      var user = new Usuario { UserName = request.UserName, Email = request.Email, NomeCompleto = request.NomeCompleto, Cargo = request.Cargo, Perfil = request.Perfil };
      var result = await _userManager.CreateAsync(user, password);
      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to create user: {errors}");
      }
      await AssignRoleAsync(user.Id, user.Perfil.ToString());
      return user;
    }

    public async Task UpdateUserAsync(string id, Usuario user)
    {
      var existingUser = await _userManager.FindByIdAsync(id);
      if (existingUser == null)
      {
        throw new KeyNotFoundException($"User with ID {id} not found.");
      }

      existingUser.UserName = user.UserName ?? user.Email;
      existingUser.Email = user.Email;
      existingUser.NomeCompleto = user.NomeCompleto;
      existingUser.Cargo = user.Cargo;

      var result = await _userManager.UpdateAsync(existingUser);
      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to update user: {errors}");
      }
    }

    public async Task DeleteUserAsync(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user != null)
      {
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
          var errors = string.Join(", ", result.Errors.Select(e => e.Description));
          throw new InvalidOperationException($"Failed to delete user: {errors}");
        }
      }
    }

    public async Task AssignRoleAsync(string userId, string role)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        throw new KeyNotFoundException($"User with ID {userId} not found.");
      }

      // Ensure role exists
      if (!await _roleManager.RoleExistsAsync(role))
      {
        await _roleManager.CreateAsync(new IdentityRole(role));
      }

      var currentRoles = await _userManager.GetRolesAsync(user);
      await _userManager.RemoveFromRolesAsync(user, currentRoles);

      var result = await _userManager.AddToRoleAsync(user, role);
      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to assign role: {errors}");
      }
    }
  }
}
