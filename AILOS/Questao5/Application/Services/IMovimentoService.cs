using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Application.Services
{
    public interface IMovimentoService
    {
        Task<MovimentarContaResponse> Adicionar(MovimentarContaRequest movimento);

        Task<IEnumerable<Movimento>> ObterPorContaCorrente(Guid idContaCorrente);
        
    }
}
