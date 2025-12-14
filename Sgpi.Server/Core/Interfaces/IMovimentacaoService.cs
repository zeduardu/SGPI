using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IMovimentacaoService
    {
        Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetByItemCatalogoIdAsync(int itemCatalogoId);
        Task RegistrarSaidaAsync(int itemCatalogoId, int quantidade, string usuarioId, string solicitante);
        Task RegistrarAjusteAsync(int itemCatalogoId, int novaQuantidade, string usuarioId, string observacao);
    }
}
