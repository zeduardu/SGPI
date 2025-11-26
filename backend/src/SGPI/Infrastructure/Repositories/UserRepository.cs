using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SgpiContext _context;

        public UserRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
