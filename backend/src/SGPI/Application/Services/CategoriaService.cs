using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
        {
            return await _categoriaRepository.GetAllAsync();
        }

        public async Task<Categoria?> GetCategoriaByIdAsync(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task<Categoria> CreateCategoriaAsync(Categoria categoria)
        {
            return await _categoriaRepository.AddAsync(categoria);
        }

        public async Task UpdateCategoriaAsync(int id, Categoria categoria)
        {
            var existingCategoria = await _categoriaRepository.GetByIdAsync(id);
            if (existingCategoria == null)
            {
                throw new KeyNotFoundException($"Categoria with ID {id} not found.");
            }

            existingCategoria.Nome = categoria.Nome;
            // Update other properties if necessary

            await _categoriaRepository.UpdateAsync(existingCategoria);
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria != null)
            {
                await _categoriaRepository.DeleteAsync(categoria);
            }
        }
    }
}
