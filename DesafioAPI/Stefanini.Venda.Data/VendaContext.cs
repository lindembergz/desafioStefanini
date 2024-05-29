// Stefanini.Venda.Data/VendaContext.cs
using Microsoft.EntityFrameworkCore;
using Stefanini.Venda.Domain.Entities;
using Stefanini.Venda.Data.Mappings;
using Stefanini.Estoque.Domain.Entities;
using Stefanini.Estoque.Data.Mappings;

namespace Stefanini.Venda.Data.Context
{
    public class VendaContext : DbContext
    {
        public VendaContext(DbContextOptions<VendaContext> options)
            : base(options)
        {
        }

        public DbSet<PedidoEntity> Pedidos { get; set; }
        public DbSet<ItemPedidoEntity> ItensPedido { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PedidoMapping());
            modelBuilder.ApplyConfiguration(new ItemPedidoMapping());
            modelBuilder.ApplyConfiguration(new ProdutoMapping());
        }

    }
}