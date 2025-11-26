using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class ItemCatalogoService : IItemCatalogoService
    {
        private readonly IItemCatalogoRepository _itemRepository;

        public ItemCatalogoService(IItemCatalogoRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemCatalogo>> GetAllItensAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<ItemCatalogo?> GetItemByIdAsync(int id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<ItemCatalogo> CreateItemAsync(ItemCatalogo item)
        {
            // Ensure Categoria is not set to avoid EF trying to insert it if it's just an ID reference
            item.Categoria = null; 
            return await _itemRepository.AddAsync(item);
        }

        public async Task UpdateItemAsync(int id, ItemCatalogo item)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }

            existingItem.Nome = item.Nome;
            existingItem.DescricaoDetalhada = item.DescricaoDetalhada;
            existingItem.UnidadeMedida = item.UnidadeMedida;
            existingItem.CategoriaId = item.CategoriaId;

            await _itemRepository.UpdateAsync(existingItem);
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item != null)
            {
                await _itemRepository.DeleteAsync(item);
            }
        }
    }
}
