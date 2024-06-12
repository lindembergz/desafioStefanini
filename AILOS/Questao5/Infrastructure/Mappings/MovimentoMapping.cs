

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questao5.Domain.Entities;

namespace Questao5.Venda.Data.Mappings
{
    public class MovimentoMapping : IEntityTypeConfiguration<Movimento>
    {
        public void Configure(EntityTypeBuilder<Movimento> builder)
        {
            builder.HasKey(p => p.IdMovimento);

            builder.Property(p => p.IdMovimento)
                .IsRequired()
                .HasColumnType("TEXT(37)");

            builder.Property(p => p.IdContaCorrente)
                .IsRequired()
                .HasColumnName("idcontacorrente")
                .HasColumnType("TEXT(37)");

            builder.Property(p => p.DataMovimento)
                .IsRequired()
                .HasColumnType("TEXT(35)");

            builder.Property(p => p.TipoMovimento)
                .IsRequired()
                .HasColumnType("TEXT(1)");

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("REAL");

            builder.HasOne(m => m.ContaCorrente)
              .WithMany(c => c.Movimentos)
              .HasForeignKey(m => m.IdContaCorrente);

            /* builder.HasOne(i => i.ContaCorrente)
               .WithMany()
               .HasForeignKey(i => i.IdContaCorrente)
               .OnDelete(DeleteBehavior.Restrict);*/

            builder.ToTable("movimento");
        }
    }
}