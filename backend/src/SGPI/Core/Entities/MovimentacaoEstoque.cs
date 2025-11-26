using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGPI.Core.Entities
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int ItemCatalogoId { get; set; }
        public ItemCatalogo? ItemCatalogo { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
        
        // Optional fields for context
        public int? OrdemDeCompraId { get; set; } // For future use
        public string? NotaFiscal { get; set; }
        public decimal? ValorUnitarioEfetivo { get; set; }
        public string? Solicitante { get; set; } // For SAIDA
        public string? ObservacaoAjuste { get; set; } // For AJUSTE_INVENTARIO
    }

    public enum TipoMovimentacao
    {
        ENTRADA,
        SAIDA,
        AJUSTE_INVENTARIO
    }
}
