

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questao5.Domain.Entities;

namespace Questao5.Venda.Data.Mappings
{
    public class IdempotenciaMapping : IEntityTypeConfiguration<Idempotencia>
    {
        public void Configure(EntityTypeBuilder<Idempotencia> builder)
        {
            builder.HasKey(p => p.ChaveIdempotencia);

            builder.Property(p => p.ChaveIdempotencia)
                .IsRequired()
                .HasColumnName("chave_idempotencia")
                .HasColumnType("TEXT(37)");

            builder.Property(p => p.Requisicao)
                .IsRequired()
                .HasColumnType("TEXT(1000)");

            builder.Property(p => p.Resultado)
                .IsRequired()
                .HasColumnType("TEXT(1000)");


            builder.ToTable("idempotencia");
        }
    }
}