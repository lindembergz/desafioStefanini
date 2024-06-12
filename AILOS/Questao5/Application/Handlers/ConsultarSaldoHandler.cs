using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Application.Validators;
using Questao5.Domain.Interfaces.Repositories;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoRequest, ConsultarSaldoResponse>
    {


        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ConsultarSaldoHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoRequest request, CancellationToken cancellationToken)
        {
            ContaCorrenteValidator.validRequest(request);

            var contaCorrente = await _contaCorrenteRepository.ObterPorNumeroConta(request.NumeroConta);

            ContaCorrenteValidator.validContaCorrente(contaCorrente);
   
           
            var movimentos = await _movimentoRepository.ObterPorContaCorrente(contaCorrente.IdContaCorrente);

            decimal creditos = movimentos.Where(m => m.TipoMovimento == TipoMovimento.Credito.ToString()[0].ToString()).Sum(m => m.Valor);
            decimal debitos = movimentos.Where(m => m.TipoMovimento == TipoMovimento.Debito.ToString()[0].ToString()).Sum(m => m.Valor);

            decimal saldo = creditos - debitos;

            return new ConsultarSaldoResponse
            {
                NumeroConta = contaCorrente.Numero.ToString(),
                NomeTitular = contaCorrente.Nome,
                DataHoraResposta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
