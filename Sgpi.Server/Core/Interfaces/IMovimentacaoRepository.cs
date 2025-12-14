using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetByItemCatalogoIdAsync(int itemCatalogoId);
        Task<MovimentacaoEstoque> AddAsync(MovimentacaoEstoque movimentacao);
        Task<IEnumerable<MovimentacaoEstoque>> GetReportAsync(DateTime? start, DateTime? end, TipoMovimentacao? type, string? userId, int? itemId);
    }
}
