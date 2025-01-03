﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Questao5.Infrastructure.Context;

#nullable disable

namespace Questao5.Migrations
{
    [DbContext(typeof(SQLiteContext))]
    partial class SQLiteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.19");

            modelBuilder.Entity("Questao5.Domain.Entities.ContaCorrente", b =>
                {
                    b.Property<Guid>("IdContaCorrente")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(37)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdContaCorrente");

                    b.ToTable("contacorrente");
                });

            modelBuilder.Entity("Questao5.Domain.Entities.Movimento", b =>
                {
                    b.Property<Guid>("IdMovimento")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(37)
                        .HasColumnType("TEXT");

                    b.Property<string>("DataMovimento")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IdContaCorrente")
                        .HasMaxLength(37)
                        .HasColumnType("TEXT");

                    b.Property<int>("TipoMovimento")
                        .HasMaxLength(1)
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT");

                    b.HasKey("IdMovimento");

                    b.HasIndex("IdContaCorrente");

                    b.ToTable("Movimento");
                });

            modelBuilder.Entity("Questao5.Domain.Entities.Movimento", b =>
                {
                    b.HasOne("Questao5.Domain.Entities.ContaCorrente", "ContaCorrente")
                        .WithMany("Movimentos")
                        .HasForeignKey("IdContaCorrente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContaCorrente");
                });

            modelBuilder.Entity("Questao5.Domain.Entities.ContaCorrente", b =>
                {
                    b.Navigation("Movimentos");
                });
#pragma warning restore 612, 618
        }
    }
}
