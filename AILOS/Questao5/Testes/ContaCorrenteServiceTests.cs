using Moq;
using Questao5.Application.Services;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Xunit;

namespace Questao5.Tests.ServiceTests
{
    public class ContaCorrenteServiceTests
    {
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
        private readonly ContaCorrenteService _contaCorrenteService;

        public ContaCorrenteServiceTests()
        {
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _contaCorrenteService = new ContaCorrenteService(null, _contaCorrenteRepositoryMock.Object);
        }

        [Fact]
        public async Task ObterPorNumeroConta_ComContaExistente_DeveDevolverContaCorrente()
        {
        
            var numeroConta = "1234567890";
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = Guid.NewGuid(),
                Numero = int.Parse(numeroConta),
                Nome = "John Doe",
                Ativo = true
            };

            _contaCorrenteRepositoryMock.Setup(r => r.ObterPorNumeroConta(numeroConta)).ReturnsAsync(contaCorrente);

           
            var result = await _contaCorrenteService.ObterPorNumeroConta(numeroConta);

         
            Assert.NotNull(result);
            Assert.Equal(contaCorrente.IdContaCorrente, result.IdContaCorrente);
            Assert.Equal(contaCorrente.Numero, result.Numero);
            Assert.Equal(contaCorrente.Nome, result.Nome);
            Assert.Equal(contaCorrente.Ativo, result.Ativo);
        }

        [Fact]
        public async Task ObterPorNumeroConta_ComContaNaoExistente_DeveRetornarNulo()
        {
           
            var numeroConta = "123";
            _contaCorrenteRepositoryMock.Setup(r => r.ObterPorNumeroConta(numeroConta)).ReturnsAsync((ContaCorrente)null);

           
            var result = await _contaCorrenteService.ObterPorNumeroConta(numeroConta);

            
            Assert.Null(result);
        }
    }
}