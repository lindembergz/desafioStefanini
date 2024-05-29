// Stefanini.Estoque.Data/EstoqueContext.cs
using Microsoft.EntityFrameworkCore;
using Stefanini.Estoque.Data.Mappings;
using Stefanini.Estoque.Domain.Entities;

namespace Stefanini.Estoque.Data.Context
{
    public class EstoqueContext : DbContext
    {
        public EstoqueContext(DbContextOptions<EstoqueContext> options)
            : base(options)
        {
        }

        public DbSet<ProdutoEntity> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMapping());
        }
    }
}