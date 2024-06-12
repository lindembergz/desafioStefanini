using Microsoft.EntityFrameworkCore;
using Questao5.Core.Data;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces.Repositories;
using Questao5.Infrastructure.Context;
using SQLitePCL;
using System.Collections.Immutable;

namespace Questao5.Infrastructure.Repositories
{
     public class MovimentoRepository : IMovimentoRepository
    {
        private readonly SQLiteContext _context;
        private readonly IUnitOfWork _unitOfWork;
        //public IUnitOfWork UnitOfWork => _unitOfWork;
        public IUnitOfWork UnitOfWork => _context;

        public MovimentoRepository(SQLiteContext context)
        {
            _context = context;

        }

        public async Task Adicionar(Movimento movimento)
        {
            await _context.MovimentosContasCorrentes.AddAsync(movimento);

            //Quem vai fazer o saveChanges será _unitOfWork
            //await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movimento>> ObterPorContaCorrente(Guid idContaCorrente)
        {
            return await _context.MovimentosContasCorrentes.Where(m => m.IdContaCorrente == idContaCorrente).ToListAsync(); 
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
