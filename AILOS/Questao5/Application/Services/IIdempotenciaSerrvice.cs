using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Application.Services
{
    public interface IIdempotenciaService
    {
        Task<(string, MovimentarContaResponse)> ChaveJaProcessada(MovimentarContaRequest request);

        Task AdicionarIdempotencia(string chaveIdempotencia, string requisicao, string resultado);
    }
}
