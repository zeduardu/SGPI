using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using System.Security.Claims;

public static class OrdemCompraEndpoints
{
    public static void MapOrdemCompraEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ordens-compra").WithTags("OrdensCompra");

        group.MapGet("/", async (IOrdemCompraService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        })
        .WithName("GetAllOrdensCompra")
        .Produces<IEnumerable<OrdemDeCompra>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, IOrdemCompraService service) =>
        {
            var oc = await service.GetByIdAsync(id);
            return oc is not null ? Results.Ok(oc) : Results.NotFound();
        })
        .WithName("GetOrdemCompraById")
        .Produces<OrdemDeCompra>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (OrdemDeCompra ordemCompra, IOrdemCompraService service) =>
        {
            var createdOc = await service.CreateAsync(ordemCompra);
            return Results.Created($"/api/ordens-compra/{createdOc.Id}", createdOc);
        })
        .WithName("CreateOrdemCompra")
        .Produces<OrdemDeCompra>(StatusCodes.Status201Created);

        group.MapPost("/{id}/approve", async (int id, IOrdemCompraService service, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? httpContext.User.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            try
            {
                await service.ApproveAsync(id, userId);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ApproveOrdemCompra")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/{id}/receive", async (int id, List<ItemRecebimentoDto> itens, IOrdemCompraService service, HttpContext httpContext) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? httpContext.User.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            try
            {
                await service.ReceiveAsync(id, userId, itens);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ReceiveOrdemCompra")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/{id}/cancel", async (int id, IOrdemCompraService service) =>
        {
            try
            {
                await service.CancelAsync(id);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CancelOrdemCompra")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
