using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Questao5.Domain.Enumerators;
using System.Drawing;

namespace Questao5.Domain.Entities
{
    [Table("Movimento")]
    public class Movimento
    {
        //[Key]
        ///[StringLength(37)]
        public Guid IdMovimento { get; private set; }

        //[Required]
        //[StringLength(37)]
        public Guid IdContaCorrente { get; private set; }

        //[ForeignKey("IdContaCorrente")]
        public virtual ContaCorrente ContaCorrente { get; set; }

        //[Required]
        //[StringLength(25)]
        public string DataMovimento { get; private set; }

        //[Required]
       // [StringLength(1)]
        public string TipoMovimento { get; private set; }

       // [Required]
        public decimal Valor { get; private set; }

        public Movimento()
        {

        }
        public Movimento(ContaCorrente _ContaCorrente, Guid _IdMovimento, Guid _IdContaCorrente, string _DataMovimento, string _TipoMovimento, decimal _Valor)
        {
            ContaCorrente = _ContaCorrente;
            IdMovimento = _IdMovimento;
            IdContaCorrente = _IdContaCorrente;
            DataMovimento = _DataMovimento;
            TipoMovimento = _TipoMovimento;
            Valor = _Valor;   
        }
    }
}
