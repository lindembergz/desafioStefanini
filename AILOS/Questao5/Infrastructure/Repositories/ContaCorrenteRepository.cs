using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Questao5.Infrastructure.Context;
using SQLitePCL;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly SQLiteContext _context;

        public ContaCorrenteRepository(SQLiteContext context)
        {
            _context = context;
        }

        public async Task<ContaCorrente> ObterPorNumeroConta(string numeroConta)
        {
            return await _context.ContasCorrentes.Where(c => c.Numero == int.Parse(numeroConta)).FirstOrDefaultAsync();
        }
    }
}
