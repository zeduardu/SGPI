using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IOrdemCompraRepository
    {
        Task<IEnumerable<OrdemDeCompra>> GetAllAsync();
        Task<OrdemDeCompra?> GetByIdAsync(int id);
        Task<OrdemDeCompra> AddAsync(OrdemDeCompra ordemCompra);
        Task UpdateAsync(OrdemDeCompra ordemCompra);
        Task<IEnumerable<OrdemDeCompra>> GetReportAsync(DateTime? start, DateTime? end, StatusOrdemDeCompra? status, string? requesterId);
    }
}
