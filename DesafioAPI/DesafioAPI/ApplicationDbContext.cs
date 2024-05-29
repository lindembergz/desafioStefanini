using Microsoft.EntityFrameworkCore;
using Stefanini.Estoque.Data.Mappings;
using Stefanini.Estoque.Domain.Entities;
using Stefanini.Venda.Data.Mappings;
using Stefanini.Venda.Domain.Entities;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PedidoEntity> Pedidos { get; set; }
    public DbSet<ItemPedidoEntity> ItensPedido { get; set; }
    public DbSet<ProdutoEntity> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PedidoMapping());
        modelBuilder.ApplyConfiguration(new ItemPedidoMapping());
        modelBuilder.ApplyConfiguration(new ProdutoMapping());
    }
}
