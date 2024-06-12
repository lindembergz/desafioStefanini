using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Validators
{
    public static class MovimentoContaValidator
    {
        public static void validRequest(MovimentarContaRequest request)
        {
            if (string.IsNullOrEmpty(request.IdRequisicao) ||
                string.IsNullOrWhiteSpace(request.IdRequisicao)
                )
                throw new MovimentoInvalidRequestException();

            int SomenteNumero = 0;
            int.TryParse(request.NumeroConta, out SomenteNumero);
            if (request.NumeroConta?.Length > 10 ||
                string.IsNullOrEmpty(request.NumeroConta) ||
                string.IsNullOrWhiteSpace(request.NumeroConta) ||
                !int.TryParse(request.NumeroConta, out SomenteNumero))
            {
                throw new MovimentoInvalidAccountException();
            }

            if (request.Valor <= 0)
                throw new MovimentoInvalidValueException();

            if (request.Tipo != TipoMovimento.Credito && request.Tipo != TipoMovimento.Debito)
                throw new MovimentoInvalidMovementTypeException();

        }
    }
}
