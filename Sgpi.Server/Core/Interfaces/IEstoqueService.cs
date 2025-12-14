using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IEstoqueService
    {
        Task<IEnumerable<Estoque>> GetAllEstoqueAsync();
        Task<Estoque?> GetEstoqueByIdAsync(int id);
        Task<Estoque?> GetEstoqueByItemCatalogoIdAsync(int itemCatalogoId);
        Task<Estoque> CreateEstoqueAsync(Estoque estoque);
        Task UpdateEstoqueAsync(int id, Estoque estoque);
        Task<IEnumerable<Estoque>> GetItemsBelowMinimumAsync();
        Task DeleteEstoqueAsync(int id);
    }
}
