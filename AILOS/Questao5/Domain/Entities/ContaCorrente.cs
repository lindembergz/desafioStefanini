using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("contacorrente")]
    public class ContaCorrente
    {
        //[Key]
        public Guid IdContaCorrente { get; set; }

        //[Required]
        public int Numero { get; set; }

        //[Required]
        //[StringLength(100)]
        public string Nome { get; set; }

        //[Required]
        public bool Ativo { get; set; }

        public ICollection<Movimento> Movimentos { get; set; }

     
    }
}
