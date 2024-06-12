using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Presentation.Controller
{
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaRequest command)
        {
           var movimentoId = await _mediator.Send(command);
           return Ok(movimentoId);          
      
        }

        [HttpGet("saldo")]
        public async Task<IActionResult> ConsultarSaldo([FromQuery] ConsultarSaldoRequest query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /* MINIMAL API
         
            endpoints.MapPost("/api/contacorrente/movimentar", async (IMediator mediator, MovimentarContaRequest command) =>
            {
                var movimentoId = await mediator.Send(command);
                return Results.Ok(movimentoId);
            });

            endpoints.MapGet("/api/contacorrente/saldo", async (IMediator mediator, [FromQuery] ConsultarSaldoRequest query) =>
            {
                var response = await mediator.Send(query);
                return Results.Ok(response);
            });


         */
    }
}
