using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Questao5.Domain.Interfaces.Services;
using Questao5.Infrastructure.Context;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Application.Services
{
    public class ContaCorrenteService: IContaCorrenteService
    {

        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        public ContaCorrenteService(SQLiteContext context, IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }
        public async Task<ContaCorrente> ObterPorNumeroConta(string numeroConta)
        {
            return await _contaCorrenteRepository.ObterPorNumeroConta(numeroConta); 
        }
    
    }
}
