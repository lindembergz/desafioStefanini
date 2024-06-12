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
        public void RequisicaoValida_ComDadosValidos_NaoDeveLancarExcecao()
        {
     
            var request = new ConsultarSaldoRequest { NumeroConta = "1234567890" };

            var exception = Record.Exception(() => ContaCorrenteValidator.validRequest(request));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123456789012")]
        [InlineData("abcdefghij")]
        public void RequisicaoValida_ComDadosInvalidos_DeveLancarContaCorrenteContaInvalidaExcecao(string numeroConta)
        {

            var request = new ConsultarSaldoRequest { NumeroConta = numeroConta };

        
            Assert.Throws<ContaCorrenteInvalidAccountException>(() => ContaCorrenteValidator.validRequest(request));
        }

        [Fact]
        public void ContaCorrenteValida_ComDadosValidos_NaoDeveLancarExcecao()
        {

            var contaCorrente = new ContaCorrente { Ativo = true };

            var exception = Record.Exception(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
            Assert.Null(exception);
        }

        [Fact]
        public void ContaCorrenteValida_ComContaCorrenteNula_DeveLancarContaCorrenteContaInvalidaExcecao()
        {
    
            ContaCorrente contaCorrente = null;


            Assert.Throws<ContaCorrenteInvalidAccountException>(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
        }

        [Fact]
        public void ContaCorrenteValida_ComContaCorrenteInativa_DeveLancarContaCorrenteContaInativaExcecao()
        {
    
            var contaCorrente = new ContaCorrente { Ativo = false };

            Assert.Throws<ContaCorrenteInactiveAccountException>(() => ContaCorrenteValidator.validContaCorrente(contaCorrente));
        }
    }
}