

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questao5.Domain.Entities;

namespace Questao5.Venda.Data.Mappings
{
    public class ContaCorrenteMapping : IEntityTypeConfiguration<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.HasKey(p => p.IdContaCorrente);

            builder.Property(p => p.IdContaCorrente)
                .IsRequired()
                .HasColumnType("TEXT(37)");

            builder.Property(p => p.Numero)
                .IsRequired()
                .HasColumnType("INTEGER(10)");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Ativo)
                .IsRequired()
                .HasColumnType("INTEGER(1)");

            builder.ToTable("contacorrente");
        }
    }
}