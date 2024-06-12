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
        private readonly Mock<IIdempotenciaService> _idempotenciaServiceMock;
        private readonly MovimentoService _movimentoService;

        public MovimentoServiceTests()
        {
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _idempotenciaServiceMock = new Mock<IIdempotenciaService>();
            _movimentoService = new MovimentoService(_movimentoRepositoryMock.Object, _contaCorrenteRepositoryMock.Object, _idempotenciaServiceMock.Object);
        }

        [Fact]
        public async Task Adicionar_ComRequisicaoValida_DeveAdicionarMovimento()
        {
           
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = Guid.NewGuid(),
                Numero = int.Parse(request.NumeroConta),
                Nome = "John Doe",
                Ativo = true
            };

            var chaveIdempotencia = GeneraterHash.GenerateSHA256Hash($"{request.IdRequisicao}|{request.NumeroConta}|{request.Valor}|{request.Tipo}");

            _contaCorrenteRepositoryMock.Setup(r => r.ObterPorNumeroConta(request.NumeroConta)).ReturnsAsync(contaCorrente);
            _idempotenciaServiceMock.Setup(s => s.ChaveJaProcessada(request)).ReturnsAsync((chaveIdempotencia, null));

            
            var result = await _movimentoService.Adicionar(request);

         
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.IdMovimento);
            _movimentoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Movimento>()), Times.Once);
            _idempotenciaServiceMock.Verify(s => s.AdicionarIdempotencia(chaveIdempotencia, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Adicionar_ComIdempotenciaProcessada_DeveRetornarResultadoExistente()
        {
          
            var request = new MovimentarContaRequest
            {
                IdRequisicao = "abc123",
                NumeroConta = "1234567890",
                Valor = 100,
                Tipo = TipoMovimento.Credito
            };

            var existingResult = new MovimentarContaResponse { IdMovimento = Guid.NewGuid() };
            var chaveIdempotencia = GeneraterHash.GenerateSHA256Hash($"{request.IdRequisicao}|{request.NumeroConta}|{request.Valor}|{request.Tipo}");

            _idempotenciaServiceMock.Setup(s => s.ChaveJaProcessada(request)).ReturnsAsync((chaveIdempotencia, existingResult));

            
            var result = await _movimentoService.Adicionar(request);

            
            Assert.Equal(existingResult.IdMovimento, result.IdMovimento);
            _movimentoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Movimento>()), Times.Never);
            _idempotenciaServiceMock.Verify(s => s.AdicionarIdempotencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}