using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Application.Validators;
using Questao5.Domain.Interfaces.Services;
using Questao5.Application.Services;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoRequest, ConsultarSaldoResponse>
    {


        private readonly IContaCorrenteService _contaCorrenteService;
        private readonly IMovimentoService _movimentoService;

        public ConsultarSaldoHandler(IContaCorrenteService contaCorrenteService, IMovimentoService movimentoService)
        {
            _contaCorrenteService = contaCorrenteService;
            _movimentoService = movimentoService;
        }

        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoRequest request, CancellationToken cancellationToken)
        {
            ContaCorrenteValidator.validRequest(request);

            var contaCorrente = await _contaCorrenteService.ObterPorNumeroConta(request.NumeroConta);

            ContaCorrenteValidator.validContaCorrente(contaCorrente);   
           
            var movimentos = await _movimentoService.ObterPorContaCorrente(contaCorrente.IdContaCorrente);

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
