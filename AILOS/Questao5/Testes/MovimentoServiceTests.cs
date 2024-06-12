using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Services;
using Questao5.Core.Security;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces.Repositories;
using Xunit;

namespace Questao5.Tests.ServiceTests
{
    public class MovimentoServiceTests
    {
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
        private readonly Mock<IIdempotenciaRepository> _idempotenciaRepositoryMock;
        private readonly MovimentoService _movimentoService;

        public MovimentoServiceTests()
        {
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _idempotenciaRepositoryMock = new Mock<IIdempotenciaRepository>();
            _movimentoService = new MovimentoService(_movimentoRepositoryMock.Object, _contaCorrenteRepositoryMock.Object, _idempotenciaRepositoryMock.Object);
        }

        [Fact]
        public async Task Adicionar_WithValidRequest_ShouldAddMovimento()
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "123",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = Guid.Parse("B6BAFC09-6967-ED11-A567-055DFA4A16C9"),
                Numero = int.Parse(request.NumeroConta),
                Nome = "John Doe",
                Ativo = true
            };

            _contaCorrenteRepositoryMock.Setup(r => r.ObterPorNumeroConta(request.NumeroConta)).ReturnsAsync(contaCorrente);
            _idempotenciaRepositoryMock.Setup(r => r.ChaveJaProcessada(It.IsAny<string>())).ReturnsAsync((Idempotencia)null);

            // Act
            var result = await _movimentoService.Adicionar(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.IdMovimento);
            _movimentoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Movimento>()), Times.Once);
            _idempotenciaRepositoryMock.Verify(r => r.AdicionarIdempotencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Adicionar_WithIdempotenciaProcessada_ShouldReturnExistingResult()
        {
            // Arrange
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "123",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            var existingResult = new MovimentarContaResponse { IdMovimento = Guid.NewGuid() };
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = GeneraterHash.GenerateSHA256Hash($"{request.IdRequisicao}|{request.NumeroConta}|{request.Valor}|{request.Tipo}"),
                Requisicao = "{...}",
                Resultado = Newtonsoft.Json.JsonConvert.SerializeObject(existingResult)
            };

            _idempotenciaRepositoryMock.Setup(r => r.ChaveJaProcessada(It.IsAny<string>())).ReturnsAsync(idempotencia);

            // Act
            var result = await _movimentoService.Adicionar(request);

            // Assert
            Assert.Equal(existingResult.IdMovimento, result.IdMovimento);
            _movimentoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Movimento>()), Times.Never);
            _idempotenciaRepositoryMock.Verify(r => r.AdicionarIdempotencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}