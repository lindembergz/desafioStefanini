using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Questao5.Resources;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Questao5.Application.Validators
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {


        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            object errorResponse = null;            

            if (exception is ContaCorrenteInvalidAccountException)
            {           
                errorResponse = new { Mensagem = Resource.ContaCorrenteInvalidAccountException, Tipo = "INVALID_ACCOUNT" };
            }
            else if (exception is ContaCorrenteInactiveAccountException)
            {
                errorResponse = new { Mensagem = Resource.ContaCorrenteInactiveAccountException, Tipo = "INACTIVE_ACCOUNT" };
            }
            else if (exception is MovimentoInvalidRequestException)
            {
                errorResponse = new { Mensagem = Resource.MovimentoInvalidRequestException, Tipo = "INVALID_REQUEST" };
            }
            else if (exception is MovimentoInvalidAccountException)
            {
                errorResponse = new { Mensagem = Resource.MovimentoInvalidAccountException, Tipo = "INVALID_ACCOUNT" };
            }
            else if (exception is MovimentoInactiveAccountException)
            {
                errorResponse = new { Mensagem = Resource.MovimentoInactiveAccountException, Tipo = "INACTIVE_ACCOUNT" };
            }
            else if (exception is MovimentoInvalidValueException)
            {
                errorResponse = new { Mensagem = Resource.MovimentoInvalidValueException, Tipo = "INVALID_VALUE" };
            }
            else if (exception is MovimentoInvalidMovementTypeException)
            {
                errorResponse = new { Mensagem = Resource.MovimentoInvalidMovementTypeException, Tipo = "INVALID_TYPE" };
            }
            else if (exception is ValidationException validationException)
            {
                errorResponse = new { Mensagem = validationException.Message };
            }
            else
            {
                errorResponse = new { Mensagem = "Erro interno.", Tipo = "INTERNAL_ERROR" };
            }

            context.Result = new BadRequestObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
