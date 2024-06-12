using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Validators;
using Questao5.Domain.Entities;
using Xunit;

namespace Questao5.Tests.ValidatorTests
{
    public class ContaCorrenteValidatorTests
    {
        [Fact]
        public void ValidRequest_WithValidData_ShouldNotThrowException()
        {
            // Arrange
            var request = new ConsultarSaldoRequest { NumeroConta = "1234567890" };

            // Act & Assert
            var exception = Record.Exception(() => ContaCorrenteValidator.validRequest(request));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123456789012")]
        [InlineData("abcdefghij")]
        public void ValidRequest_WithInvalidData_ShouldThrowContaCorrenteInvalidAccountException(string numeroConta)
        {
            // Arrange
            var request = new ConsultarSaldoRequest { NumeroConta = numeroConta };

            // Act & Assert
            Assert.Throws<ContaCorrenteInvalidAccountException>(() => ContaCorrenteValidator.validRequest(request));
        }

        [Fact]
        public void ValidContaCorrente_WithValidData_ShouldNotThrowException()
        {
            // Arrange
            var contaCorrente = new ContaCorrente { Ativo = true };

            // Act & Assert
            var exception = Record.Exception(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidContaCorrente_WithNullContaCorrente_ShouldThrowContaCorrenteInvalidAccountException()
        {
            // Arrange
            ContaCorrente contaCorrente = null;

            // Act & Assert
            Assert.Throws<ContaCorrenteInvalidAccountException>(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
        }

        [Fact]
        public void ValidContaCorrente_WithInativeContaCorrente_ShouldThrowContaCorrenteInactiveAccountException()
        {
            // Arrange
            var contaCorrente = new ContaCorrente { Ativo = false };

            // Act & Assert
            Assert.Throws<ContaCorrenteInactiveAccountException>(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
        }
    }
}