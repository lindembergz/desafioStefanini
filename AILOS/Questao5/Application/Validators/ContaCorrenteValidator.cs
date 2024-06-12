using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Validators
{
    public static class ContaCorrenteValidator
    {

        public static void validRequest(ConsultarSaldoRequest request)
        {
            int SomenteNumero = 0;
            int.TryParse(request.NumeroConta, out SomenteNumero);
            if (request.NumeroConta?.Length > 10 ||
                string.IsNullOrEmpty(request.NumeroConta) ||
                string.IsNullOrWhiteSpace(request.NumeroConta) ||
                !int.TryParse(request.NumeroConta, out SomenteNumero))
            {
                throw new ContaCorrenteInvalidAccountException();
            }
        }

        public static void validContaCorrente(ContaCorrente contaCorrente)
        {
            if (contaCorrente == null)
                throw new ContaCorrenteInvalidAccountException();

            if (!contaCorrente.Ativo)
                throw new ContaCorrenteInactiveAccountException();
        }

    }
}
