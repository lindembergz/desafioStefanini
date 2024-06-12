using Questao5.Application.Commands.Requests;
using Questao5.Application.Validators;
using Questao5.Domain.Enumerators;
using Xunit;

namespace Questao5.Tests.ValidatorTests
{
    public class MovimentoContaValidatorTests
    {
        [Fact]
        public void ValidRequest_WithValidData_ShouldNotThrowException()
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            // Act & Assert
            var exception = Record.Exception(() => MovimentoContaValidator.validRequest(request));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidRequest_WithInvalidIdRequisicao_ShouldThrowMovimentoInvalidRequestException(string idRequisicao)
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = idRequisicao,
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            // Act & Assert
            Assert.Throws<MovimentoInvalidRequestException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123456789012")]
        [InlineData("abcdefghij")]
        public void ValidRequest_WithInvalidNumeroConta_ShouldThrowMovimentoInvalidAccountException(string numeroConta)
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = numeroConta,
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            // Act & Assert
            Assert.Throws<MovimentoInvalidAccountException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void ValidRequest_WithInvalidValor_ShouldThrowMovimentoInvalidValueException(decimal valor)
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = valor,
                Tipo = TipoMovimento.Credito
            };

            // Act & Assert
            Assert.Throws<MovimentoInvalidValueException>(() => MovimentoContaValidator.validRequest(request));
        }

        [Theory]
        [InlineData('A')]
        [InlineData('B')]
        public void ValidRequest_WithInvalidTipoMovimento_ShouldThrowMovimentoInvalidMovementTypeException(char tipo)
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = (TipoMovimento)tipo
            };

            // Act & Assert
            Assert.Throws<MovimentoInvalidMovementTypeException>(() => MovimentoContaValidator.validRequest(request));
        }
    }
}
