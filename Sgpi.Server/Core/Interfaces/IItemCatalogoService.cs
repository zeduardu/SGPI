using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IItemCatalogoService
    {
        Task<IEnumerable<ItemCatalogo>> GetAllItensAsync();
        Task<ItemCatalogo?> GetItemByIdAsync(int id);
        Task<ItemCatalogo> CreateItemAsync(ItemCatalogo item);
        Task UpdateItemAsync(int id, ItemCatalogo item);
        Task DeleteItemAsync(int id);
    }
}
