using SGPI.Core.Entities;

namespace SGPI.Application.DTOs
{
    public class UserDto : Usuario
    {
        public List<string> Roles { get; set; } = new List<string>();
    }
}
