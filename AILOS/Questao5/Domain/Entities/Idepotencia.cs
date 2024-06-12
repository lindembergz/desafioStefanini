using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    //[Table("Idempotencia")]
    public class Idempotencia
    {
        //[Key]
        //[StringLength(37)]
        public string ChaveIdempotencia { get; private set; }

        //[Required]
        //[StringLength(1000)]
        public string Requisicao { get; private set; }

        //[Required]
        //[StringLength(1000)]
        public string Resultado { get; private set; }

        public Idempotencia(string chaveIdempotencia, string requisicao, string resultado) {

            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }
    }
}
