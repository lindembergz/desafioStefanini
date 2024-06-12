using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Questao5.Infrastructure.Context;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly SQLiteContext _context;

        public IdempotenciaRepository(SQLiteContext context)
        {
            _context = context;
        }

        public async Task<Idempotencia> ChaveJaProcessada(string chaveIdempotencia)
        {
            return await _context.Idempotencias.SingleOrDefaultAsync(i => i.ChaveIdempotencia == chaveIdempotencia);
        }

        public async Task AdicionarIdempotencia(string chaveIdempotencia, string requisicao, string resultado)
        {
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = chaveIdempotencia,
                Requisicao = requisicao,
                Resultado = resultado
            };

            await _context.Idempotencias.AddAsync(idempotencia);
            //await _context.SaveChangesAsync();
        }
    }
}
