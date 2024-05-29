using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stefanini.Estoque.Domain.Entities;

namespace Stefanini.Estoque.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<ProdutoEntity>
    {
       public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.NomeProduto)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Valor)
                .IsRequired()
                .HasColumnType("decimal");
 

            builder.ToTable("Produto");
        }
    }
}