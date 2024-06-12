using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Core.Security;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;

namespace Questao5.Application.Services
{
    public class IdempotenciaService: IIdempotenciaService
    {
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        public IdempotenciaService(IIdempotenciaRepository idempotenciaRepository)
        {
            _idempotenciaRepository = idempotenciaRepository;
        }
        private string GerarChaveIdempotencia(MovimentarContaRequest request)
        {
            string chaveIdempotencia = $"{request.IdRequisicao}|{request.NumeroConta}|{request.Valor}|{request.Tipo}";
            return GeneraterHash.GenerateSHA256Hash(chaveIdempotencia);
        }

        public async Task<(string ,MovimentarContaResponse)> ChaveJaProcessada(MovimentarContaRequest request)
        {
            var chaveIdempotencia = GerarChaveIdempotencia(request);

            var idempontencia = await _idempotenciaRepository.ChaveJaProcessada(chaveIdempotencia);
            if (idempontencia != null)
            {
                return (chaveIdempotencia, JsonConvert.DeserializeObject<MovimentarContaResponse>(idempontencia.Resultado));
            }
            return (chaveIdempotencia, null);
        }

        public async Task AdicionarIdempotencia(string chaveIdempotencia, string requisicao, string resultado) 
        {
            await _idempotenciaRepository.AdicionarIdempotencia(chaveIdempotencia, requisicao, resultado);
        }
    }
}
