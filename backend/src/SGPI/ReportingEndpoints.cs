using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using System.Security.Claims;

public static class ReportingEndpoints
{
    public static void MapReportingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/reports").WithTags("Reporting").RequireAuthorization();

        group.MapGet("/movements", async (
            DateTime? start, 
            DateTime? end, 
            TipoMovimentacao? type, 
            string? userId, 
            int? itemId,
            IMovimentacaoRepository repository) =>
        {
            // Ensure end date includes the whole day if provided
            if (end.HasValue) end = end.Value.Date.AddDays(1).AddTicks(-1);

            return Results.Ok(await repository.GetReportAsync(start, end, type, userId, itemId));
        })
        .WithName("GetMovementReport")
        .Produces<IEnumerable<MovimentacaoEstoque>>(StatusCodes.Status200OK);

        group.MapGet("/orders", async (
            DateTime? start, 
            DateTime? end, 
            StatusOrdemDeCompra? status, 
            string? requesterId,
            IOrdemCompraRepository repository) =>
        {
            // Ensure end date includes the whole day if provided
            if (end.HasValue) end = end.Value.Date.AddDays(1).AddTicks(-1);

            return Results.Ok(await repository.GetReportAsync(start, end, status, requesterId));
        })
        .WithName("GetOrderReport")
        .Produces<IEnumerable<OrdemDeCompra>>(StatusCodes.Status200OK);
    }
}
