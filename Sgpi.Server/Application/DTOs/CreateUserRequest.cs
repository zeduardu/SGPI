using SGPI.Core.Entities;

public record CreateUserRequest(
    string UserName, 
    string Email, 
    string Password, 
    string NomeCompleto, 
    Perfil Perfil, 
    string Cargo
);
