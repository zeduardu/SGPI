using System.Threading.Tasks;

namespace SGPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string email, string password);
        Task<string> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
