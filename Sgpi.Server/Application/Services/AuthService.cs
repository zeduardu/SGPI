using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sgpi.Server.Infrastructure.ExternalServices.Email;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
  public class AuthService : IAuthService
  {
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<Usuario> userManager, IConfiguration configuration, IEmailService emailService)
    {
      _userManager = userManager;
      _configuration = configuration;
      _emailService = emailService;
    }

    public async Task<string?> AuthenticateAsync(string email, string password)
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null || !await _userManager.CheckPasswordAsync(user, password))
      {
        return null;
      }

      var authClaims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Use User ID here
        new Claim(ClaimTypes.NameIdentifier, user.Id), // Also add standard NameIdentifier
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var userRoles = await _userManager.GetRolesAsync(user);
      foreach (var role in userRoles)
      {
        authClaims.Add(new Claim(ClaimTypes.Role, role));
      }

      var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

      var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.Now.AddHours(3),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return string.Empty;
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetUrl = "https://localhost:40443/api/auth/reset-password";
        var param = new Dictionary<string, string?>
        {
            {"token", resetToken },
            {"email", email }
        };
        var resetLink = QueryHelpers.AddQueryString(resetUrl, param);

        return resetLink;
    }


    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }
  }
}