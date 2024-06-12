using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    //[Table("Idempotencia")]
    public class Idempotencia
    {
        //[Key]
        //[StringLength(37)]
        public string ChaveIdempotencia { get; set; }

        //[Required]
        //[StringLength(1000)]
        public string Requisicao { get; set; }

        //[Required]
        //[StringLength(1000)]
        public string Resultado { get; set; }
    }
}
