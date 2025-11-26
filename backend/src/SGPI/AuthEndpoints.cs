using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/login", async (
            [FromBody] LoginRequest request,
            IAuthService authService) =>
        {
            var token = await authService.AuthenticateAsync(request.Email, request.Password);
            if (token == null)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(new { Token = token });
        })
        .WithName("Login")
        .Produces<LoginResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        routes.MapPost("/api/auth/register", async (
            [FromBody] RegisterRequest request,
            UserManager<Usuario> userManager) =>
        {
            var user = new Usuario { UserName = request.Email, Email = request.Email, NomeCompleto = request.NomeCompleto, Perfil = request.Perfil };
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Results.Ok(new { Message = "User registered successfully" });
            }
            return Results.BadRequest(result.Errors);
        })
        .WithName("Register")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token);
public record RegisterRequest(string Email, string Password, string NomeCompleto, Perfil Perfil);
