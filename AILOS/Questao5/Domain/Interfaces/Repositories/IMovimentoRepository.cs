using Questao5.Core.Data;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IMovimentoRepository: IRepository
    {
        Task Adicionar(Movimento movimento);
        Task<IEnumerable<Movimento>> ObterPorContaCorrente(Guid idContaCorrente);

    }
}