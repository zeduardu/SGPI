using Microsoft.AspNetCore.Identity;

namespace SGPI.Core.Entities
{
    public class Usuario : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public Perfil Perfil { get; set; }
    }

    public enum Perfil
    {
        Admin,
        OperadorNTI,
        GestorNTI
    }
}
