using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueService(IEstoqueRepository estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        public async Task<IEnumerable<Estoque>> GetAllEstoqueAsync()
        {
            return await _estoqueRepository.GetAllAsync();
        }

        public async Task<Estoque?> GetEstoqueByIdAsync(int id)
        {
            return await _estoqueRepository.GetByIdAsync(id);
        }

        public async Task<Estoque?> GetEstoqueByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _estoqueRepository.GetByItemCatalogoIdAsync(itemCatalogoId);
        }

        public async Task<Estoque> CreateEstoqueAsync(Estoque estoque)
        {
            // Ensure ItemCatalogo is not set to avoid EF trying to insert it if it's just an ID reference
            estoque.ItemCatalogo = null;
            return await _estoqueRepository.AddAsync(estoque);
        }

        public async Task UpdateEstoqueAsync(int id, Estoque estoque)
        {
            var existingEstoque = await _estoqueRepository.GetByIdAsync(id);
            if (existingEstoque == null)
            {
                throw new KeyNotFoundException("Estoque not found");
            }

            await _estoqueRepository.UpdateAsync(estoque);
        }

        public async Task<IEnumerable<Estoque>> GetItemsBelowMinimumAsync()
        {
            return await _estoqueRepository.GetItemsBelowMinimumAsync();
        }

        public async Task DeleteEstoqueAsync(int id)
        {
            var estoque = await _estoqueRepository.GetByIdAsync(id);
            if (estoque != null)
            {
                await _estoqueRepository.DeleteAsync(estoque);
            }
        }
    }
}
