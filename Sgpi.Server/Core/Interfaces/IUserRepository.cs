using SGPI.Core.Entities;
using System.Threading.Tasks;

namespace SGPI.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<Usuario?> GetUserByIdAsync(string id);
    }
}
