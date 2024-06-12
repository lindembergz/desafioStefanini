using Questao5.Application.Commands.Requests;
using Questao5.Application.Validators;
using Questao5.Domain.Enumerators;
using Xunit;

namespace Questao5.Tests.ValidatorTests
{
    public class MovimentoContaValidatorTests
    {
        [Fact]
        public void RequisicaoValida_ComDadosValidos_NaoDeveLancarExcecao()
        {
           
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            
            var exception = Record.Exception(() => MovimentoContaValidator.validRequest(request));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RequisicaoValida_ComIdRequisicaoInvalido_DeveLancarMovimentoRequisicaoInvalidaExcecao(string idRequisicao)
        {
            
            var request = new MovimentarContaRequest
            {
                IdRequisicao = idRequisicao,
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

           
            Assert.Throws<MovimentoInvalidRequestException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123456789012")]
        [InlineData("abcdefghij")]
        public void RequisicaoValida_ComNumeroContaInvalido_DeveLancarMovimentoContaInvalidaExcecao(string numeroConta)
        {
           
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = numeroConta,
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

           
            Assert.Throws<MovimentoInvalidAccountException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void RequisicaoValida_ComValorInvalido_DeveLancarMovimentoValorInvalidoExcecao(decimal valor)
        {
            
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = valor,
                Tipo = TipoMovimento.Credito
            };

           
            Assert.Throws<MovimentoInvalidValueException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData('A')]
        [InlineData('B')]
        public void RequisicaoValida_ComTipoMovimentoInvalido_DeveLancarMovimentoTipoMovimentoInvalidoExcecao(char tipo)
        {
           
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = (TipoMovimento)tipo
            };

            
            Assert.Throws<MovimentoInvalidMovementTypeException>(() => MovimentoContaValidator.validRequest(request));
        }
    }
}
