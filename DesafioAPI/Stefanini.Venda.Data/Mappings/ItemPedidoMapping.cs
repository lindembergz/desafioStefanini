// Stefanini.Venda.Data/Mappings/ItemPedidoMapping.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stefanini.Venda.Domain.Entities;

namespace Stefanini.Venda.Data.Mappings
{
    public class ItemPedidoMapping : IEntityTypeConfiguration<ItemPedidoEntity>
    {
        public void Configure(EntityTypeBuilder<ItemPedidoEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.IdPedido)
                .IsRequired();

            builder.Property(i => i.IdProduto)
                .IsRequired();

            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasColumnType("decimal(12,3)");

            builder.Property(i => i.ValorUnitario)
                .IsRequired()
                .HasColumnType("decimal(15,2)");

            builder.HasOne(i => i.Produto)
              .WithMany()
              .HasForeignKey(i => i.IdProduto)
              .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("ItemPedido");
        }
    }
}