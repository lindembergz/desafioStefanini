using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Questao5.Core.Data;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Venda.Data.Mappings;
using System.Collections.Generic;

namespace Questao5.Infrastructure.Context
{
    public class SQLiteContext : DbContext, IUnitOfWork
    {
        public SQLiteContext(DbContextOptions<SQLiteContext> options)
            : base(options)
        {

        }
        public DbSet<Movimento> MovimentosContasCorrentes { get; set; }
        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<Idempotencia> Idempotencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaCorrenteMapping());
            modelBuilder.ApplyConfiguration(new MovimentoMapping());
            modelBuilder.ApplyConfiguration(new IdempotenciaMapping());            
        }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;     
            return sucesso;
        }

    }
}
