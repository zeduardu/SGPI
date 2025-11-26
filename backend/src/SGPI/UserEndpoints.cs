using SGPI.Application.DTOs;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization("Admin");

        group.MapGet("/", async (IUserService service) =>
        {
            return Results.Ok(await service.GetAllUsersAsync());
        })
        .WithName("GetAllUsers")
        .Produces<IEnumerable<UserDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (string id, IUserService service) =>
        {
            var user = await service.GetUserByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserById")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateUserRequest request, IUserService service) =>
        {
            var user = new Usuario
            {
                UserName = request.Email, // Using Email as UserName for simplicity
                Email = request.Email,
                NomeCompleto = request.NomeCompleto,
                Cargo = request.Cargo
            };

            try
            {
                var createdUser = await service.CreateUserAsync(user, request.Password);
                return Results.Created($"/api/users/{createdUser.Id}", createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateUser")
        .Produces<Usuario>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id}", async (string id, UpdateUserRequest request, IUserService service) =>
        {
            var user = new Usuario
            {
                UserName = request.Email,
                Email = request.Email,
                NomeCompleto = request.NomeCompleto,
                Cargo = request.Cargo
            };

            try
            {
                await service.UpdateUserAsync(id, user);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (string id, IUserService service) =>
        {
            try
            {
                await service.DeleteUserAsync(id);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest);
        group.MapPut("/{id}/role", async (string id, AssignRoleRequest request, IUserService service) =>
        {
            try
            {
                await service.AssignRoleAsync(id, request.Role);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("AssignRole")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);
    }
}

public record CreateUserRequest(string Email, string Password, string NomeCompleto, string Cargo);
public record UpdateUserRequest(string Email, string NomeCompleto, string Cargo);
public record AssignRoleRequest(string Role);
