using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Validators;
using Questao5.Core.Data;
using Questao5.Core.Security;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;


namespace Questao5.Application.Services
{
    public class MovimentoService : IMovimentoService
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentoService(IMovimentoRepository movimentoRepository,
                        IContaCorrenteRepository contaCorrenteRepository,
                        IIdempotenciaRepository idempotenciaRepository
 
                            )
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        private string GerarChaveIdempotencia(MovimentarContaRequest request)
        {
            string chaveIdempotencia = $"{request.IdRequisicao}|{request.NumeroConta}|{request.Valor}|{request.Tipo}";
            return GeneraterHash.GenerateSHA256Hash(chaveIdempotencia);
        }

        public async Task<MovimentarContaResponse> Adicionar(MovimentarContaRequest request)
        {

            var chaveIdempotencia = GerarChaveIdempotencia(request);

            var idempontencia = await _idempotenciaRepository.ChaveJaProcessada(chaveIdempotencia);
            if (idempontencia != null)
            {
                return JsonConvert.DeserializeObject<MovimentarContaResponse>(idempontencia.Resultado);
            }

            var contaCorrente = await _contaCorrenteRepository.ObterPorNumeroConta(request.NumeroConta);

            ContaCorrenteValidator.validContaCorrente(contaCorrente);

            var movimento = new Movimento
                (
                 contaCorrente,
                 Guid.NewGuid(),
                 contaCorrente.IdContaCorrente,
                 DateTime.Now.ToString(),
                 request.Tipo.ToString()[0].ToString(),
                 request.Valor);

            var resultado = new MovimentarContaResponse { IdMovimento = movimento.IdMovimento };

            await _movimentoRepository.Adicionar(movimento);
            await _idempotenciaRepository.AdicionarIdempotencia(chaveIdempotencia, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(resultado));


            if (_movimentoRepository.UnitOfWork != null)
            {
                _movimentoRepository.UnitOfWork.Commit();
            }
            return resultado;
        }


        public async Task<IEnumerable<Movimento>> ObterPorContaCorrente(Guid idContaCorrente)
        {
            return await _movimentoRepository.ObterPorContaCorrente(idContaCorrente);

        }
    }
}