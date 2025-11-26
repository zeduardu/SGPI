namespace SGPI.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? NomeCompleto { get; set; }
        public string? Cargo { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
