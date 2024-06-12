using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoRequest : IRequest<ConsultarSaldoResponse>
    {
        public string NumeroConta{ get; set; }

    }
}
