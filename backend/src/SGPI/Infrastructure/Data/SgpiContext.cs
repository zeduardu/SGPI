using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;

namespace SGPI.Infrastructure.Data
{
    public class SgpiContext : IdentityDbContext<Usuario>
    {
        public SgpiContext(DbContextOptions<SgpiContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ItemCatalogo> ItensCatalogo { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<CotacaoItem> CotacoesItens { get; set; }
        public DbSet<OrdemDeCompra> OrdensDeCompra { get; set; }
        public DbSet<ItemOrdemDeCompra> ItensOrdensDeCompra { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Estoque>()
                .HasIndex(e => e.ItemCatalogoId)
                .IsUnique();
        }
    }
}
