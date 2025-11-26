using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Application.Services
{
    public class OrdemCompraService : IOrdemCompraService
    {
        private readonly IOrdemCompraRepository _repository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly SgpiContext _context;

        public OrdemCompraService(
            IOrdemCompraRepository repository,
            IEstoqueRepository estoqueRepository,
            IMovimentacaoRepository movimentacaoRepository,
            SgpiContext context)
        {
            _repository = repository;
            _estoqueRepository = estoqueRepository;
            _movimentacaoRepository = movimentacaoRepository;
            _context = context;
        }

        public async Task<IEnumerable<OrdemDeCompra>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrdemDeCompra?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<OrdemDeCompra> CreateAsync(OrdemDeCompra ordemCompra)
        {
            ordemCompra.Status = StatusOrdemDeCompra.ABERTA;
            ordemCompra.DataCriacao = DateTime.UtcNow;
            return await _repository.AddAsync(ordemCompra);
        }

        public async Task ApproveAsync(int id, string usuarioAprovadorId)
        {
            var oc = await _repository.GetByIdAsync(id);
            if (oc == null) throw new InvalidOperationException("Ordem de Compra não encontrada.");
            if (oc.Status != StatusOrdemDeCompra.ABERTA) throw new InvalidOperationException("Apenas OCs abertas podem ser aprovadas.");

            oc.Status = StatusOrdemDeCompra.APROVADA;
            oc.UsuarioAprovadorId = usuarioAprovadorId;
            oc.DataAprovacao = DateTime.UtcNow;
            
            await _repository.UpdateAsync(oc);
        }

        public async Task ReceiveAsync(int id, string usuarioId, List<ItemRecebimentoDto> itensRecebidos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var oc = await _repository.GetByIdAsync(id);
                if (oc == null) throw new InvalidOperationException("Ordem de Compra não encontrada.");
                if (oc.Status != StatusOrdemDeCompra.APROVADA && oc.Status != StatusOrdemDeCompra.CONCLUIDA_PARCIAL) 
                    throw new InvalidOperationException("Apenas OCs aprovadas ou parcialmente concluídas podem ser recebidas.");

                foreach (var recebimento in itensRecebidos)
                {
                    var itemOc = oc.Itens.FirstOrDefault(i => i.ItemCatalogoId == recebimento.ItemCatalogoId);
                    if (itemOc == null) continue; // Or throw error

                    // Update Stock
                    var estoque = await _estoqueRepository.GetByItemCatalogoIdAsync(recebimento.ItemCatalogoId);
                    if (estoque == null)
                    {
                        estoque = new Estoque 
                        { 
                            ItemCatalogoId = recebimento.ItemCatalogoId, 
                            QuantidadeEmEstoque = 0,
                            EstoqueMinimo = 0,
                            EstoqueMaximo = 100 
                        };
                        await _estoqueRepository.AddAsync(estoque);
                    }

                    estoque.QuantidadeEmEstoque += recebimento.Quantidade;
                    await _estoqueRepository.UpdateAsync(estoque);

                    // Create Movement
                    var movimentacao = new MovimentacaoEstoque
                    {
                        ItemCatalogoId = recebimento.ItemCatalogoId,
                        TipoMovimentacao = TipoMovimentacao.ENTRADA,
                        Quantidade = recebimento.Quantidade,
                        Data = DateTime.UtcNow,
                        UsuarioId = usuarioId,
                        OrdemDeCompraId = oc.Id,
                        ValorUnitarioEfetivo = recebimento.ValorUnitario
                    };
                    await _movimentacaoRepository.AddAsync(movimentacao);

                    // Update Item Received Qty
                    itemOc.QuantidadeRecebida += recebimento.Quantidade;
                }

                // Check Status
                bool allReceived = oc.Itens.All(i => i.QuantidadeRecebida >= i.QuantidadeSolicitada);
                oc.Status = allReceived ? StatusOrdemDeCompra.CONCLUIDA_TOTAL : StatusOrdemDeCompra.CONCLUIDA_PARCIAL;

                await _repository.UpdateAsync(oc);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CancelAsync(int id)
        {
            var oc = await _repository.GetByIdAsync(id);
            if (oc == null) throw new InvalidOperationException("Ordem de Compra não encontrada.");
            if (oc.Status == StatusOrdemDeCompra.CONCLUIDA_TOTAL || oc.Status == StatusOrdemDeCompra.CONCLUIDA_PARCIAL) throw new InvalidOperationException("Não é possível cancelar uma OC já recebida.");

            oc.Status = StatusOrdemDeCompra.CANCELADA;
            await _repository.UpdateAsync(oc);
        }
    }
}
