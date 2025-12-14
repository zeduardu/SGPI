using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface ICotacaoItemRepository
    {
        Task<IEnumerable<CotacaoItem>> GetAllAsync();
        Task<CotacaoItem?> GetByIdAsync(int id);
        Task<IEnumerable<CotacaoItem>> GetByItemCatalogoIdAsync(int itemCatalogoId);
        Task<CotacaoItem> AddAsync(CotacaoItem cotacao);
        Task UpdateAsync(CotacaoItem cotacao);
        Task DeleteAsync(CotacaoItem cotacao);
    }
}
