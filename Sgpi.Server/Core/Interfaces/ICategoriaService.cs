using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllCategoriasAsync();
        Task<Categoria?> GetCategoriaByIdAsync(int id);
        Task<Categoria> CreateCategoriaAsync(Categoria categoria);
        Task UpdateCategoriaAsync(int id, Categoria categoria);
        Task DeleteCategoriaAsync(int id);
    }
}
