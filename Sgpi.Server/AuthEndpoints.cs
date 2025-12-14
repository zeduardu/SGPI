using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGPI.Application.DTOs;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class AuthEndpoints
{
  public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
  {
    // login route
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

    // user register route
    routes.MapPost("/api/auth/register", async (
            [FromBody] CreateUserRequest request,
            IUserService userService) =>
    {
      try
      {
        await userService.CreateUserAsync(request, request.Password);
        return Results.Ok(new { Message = "User registered successfully" });
      }
      catch (InvalidOperationException ex)
      {
        return Results.BadRequest(ex.Message);
      }
    })
    .WithName("Register")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);
  }
}

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token);
