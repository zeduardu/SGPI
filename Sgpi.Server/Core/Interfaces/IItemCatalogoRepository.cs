using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IItemCatalogoRepository
    {
        Task<IEnumerable<ItemCatalogo>> GetAllAsync();
        Task<ItemCatalogo?> GetByIdAsync(int id);
        Task<ItemCatalogo> AddAsync(ItemCatalogo item);
        Task UpdateAsync(ItemCatalogo item);
        Task DeleteAsync(ItemCatalogo item);
    }
}
