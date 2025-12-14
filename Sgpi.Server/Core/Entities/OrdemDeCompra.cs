using System;
using System.Collections.Generic;

namespace SGPI.Core.Entities
{
    public class OrdemDeCompra
    {
        public int Id { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public string? UsuarioSolicitanteId { get; set; }
        public Usuario? UsuarioSolicitante { get; set; }
        public string? UsuarioAprovadorId { get; set; }
        public Usuario? UsuarioAprovador { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public StatusOrdemDeCompra Status { get; set; } = StatusOrdemDeCompra.ABERTA;
        public string? Observacao { get; set; }
        public List<ItemOrdemDeCompra> Itens { get; set; } = new List<ItemOrdemDeCompra>();
    }

    public enum StatusOrdemDeCompra
    {
        ABERTA,
        APROVADA,
        CONCLUIDA_PARCIAL,
        CONCLUIDA_TOTAL,
        CANCELADA,
        REJEITADA
    }
}
