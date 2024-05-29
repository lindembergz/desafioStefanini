// Stefanini.Venda.Data/Mappings/PedidoMapping.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stefanini.Venda.Domain.Entities;

namespace Stefanini.Venda.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<PedidoEntity>
    {
        public void Configure(EntityTypeBuilder<PedidoEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.NomeCliente)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.EmailCliente)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Pago)
                .IsRequired();

            builder.Property(p => p.ValorTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(p => p.PedidoItems)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.IdPedido);

            builder.ToTable("Pedido");
        }
    }
}