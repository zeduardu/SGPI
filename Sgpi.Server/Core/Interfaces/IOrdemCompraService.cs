using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IOrdemCompraService
    {
        Task<IEnumerable<OrdemDeCompra>> GetAllAsync();
        Task<OrdemDeCompra?> GetByIdAsync(int id);
        Task<OrdemDeCompra> CreateAsync(OrdemDeCompra ordemCompra);
        Task ApproveAsync(int id, string usuarioAprovadorId);
        Task ReceiveAsync(int id, string usuarioId, List<ItemRecebimentoDto> itensRecebidos);
        Task CancelAsync(int id);
    }

    public class ItemRecebimentoDto
    {
        public int ItemCatalogoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
