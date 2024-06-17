using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Services;
using Questao5.Application.Validators;
using Questao5.Core.Security;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaRequest, MovimentarContaResponse>
    {
        private readonly IMovimentoService _movimentoService;

        public MovimentarContaHandler(
              IMovimentoService movimentoService
              )
        {
            _movimentoService = movimentoService;
        }

        public async Task<MovimentarContaResponse> Handle(MovimentarContaRequest request, CancellationToken cancellationToken)
        {
            MovimentoContaValidator.validRequest(request);

            return await _movimentoService.Adicionar(request);         
        }
    }
}
