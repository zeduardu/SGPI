using System.Threading.Tasks;

namespace SGPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string email, string password);
    }
}
