using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<IEnumerable<Estoque>> GetAllAsync();
        Task<Estoque?> GetByIdAsync(int id);
        Task<Estoque?> GetByItemCatalogoIdAsync(int itemCatalogoId);
        Task<Estoque> AddAsync(Estoque estoque);
        Task UpdateAsync(Estoque estoque);
        Task<IEnumerable<Estoque>> GetItemsBelowMinimumAsync();
        Task DeleteAsync(Estoque estoque);
    }
}
