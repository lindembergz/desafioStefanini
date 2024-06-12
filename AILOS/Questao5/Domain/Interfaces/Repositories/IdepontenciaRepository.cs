using Questao5.Core.Data;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IIdempotenciaRepository 
    {
        Task<Idempotencia> ChaveJaProcessada(string chaveIdempotencia);
        Task AdicionarIdempotencia(string chaveIdempotencia, string requisicao, string resultado);
    }
}
