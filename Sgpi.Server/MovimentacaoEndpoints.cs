using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using System.Security.Claims;

public static class MovimentacaoEndpoints
{
    public static void MapMovimentacaoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/movimentacoes").WithTags("Movimentacoes");

        group.MapGet("/", async (IMovimentacaoService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        })
        .WithName("GetAllMovimentacoes")
        .Produces<IEnumerable<MovimentacaoEstoque>>(StatusCodes.Status200OK);

        group.MapGet("/item/{itemCatalogoId}", async (int itemCatalogoId, IMovimentacaoService service) =>
        {
            var movimentacoes = await service.GetByItemCatalogoIdAsync(itemCatalogoId);
            return Results.Ok(movimentacoes);
        })
        .WithName("GetMovimentacoesByItemCatalogoId")
        .Produces<IEnumerable<MovimentacaoEstoque>>(StatusCodes.Status200OK);

        group.MapPost("/saida", async (RegistrarSaidaRequest request, IMovimentacaoService service, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? httpContext.User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                userId = request.UserId;
            }

            // if (string.IsNullOrEmpty(userId))
            // {
            //      return Results.Unauthorized();
            // }

            try
            {
                if (userId == null) return Results.Unauthorized();
                await service.RegistrarSaidaAsync(request.ItemCatalogoId, request.Quantidade, userId, request.Solicitante);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("RegistrarSaida")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/ajuste", async (RegistrarAjusteRequest request, IMovimentacaoService service, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? httpContext.User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                userId = request.UserId;
            }

            if (string.IsNullOrEmpty(userId))
            {
                 return Results.Unauthorized();
            }

            try
            {
                await service.RegistrarAjusteAsync(request.ItemCatalogoId, request.NovaQuantidade, userId, request.Observacao);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("RegistrarAjuste")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}

public record RegistrarSaidaRequest(int ItemCatalogoId, int Quantidade, string Solicitante, string? UserId = null);
public record RegistrarAjusteRequest(int ItemCatalogoId, int NovaQuantidade, string Observacao, string? UserId = null);
