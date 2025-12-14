using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface ICotacaoItemService
    {
        Task<IEnumerable<CotacaoItem>> GetAllCotacoesAsync();
        Task<CotacaoItem?> GetCotacaoByIdAsync(int id);
        Task<IEnumerable<CotacaoItem>> GetCotacoesByItemCatalogoIdAsync(int itemCatalogoId);
        Task<CotacaoItem> CreateCotacaoAsync(CotacaoItem cotacao);
        Task UpdateCotacaoAsync(int id, CotacaoItem cotacao);
        Task DeleteCotacaoAsync(int id);
    }
}
