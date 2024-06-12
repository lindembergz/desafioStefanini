using Questao5.Application.Commands.Requests;
using Questao5.Core.Data;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IContaCorrenteRepository 
    {
        Task<ContaCorrente> ObterPorNumeroConta(string numeroConta);
    }
}