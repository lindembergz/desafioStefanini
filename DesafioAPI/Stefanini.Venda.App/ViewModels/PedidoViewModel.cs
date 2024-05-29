using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Venda.App.ViewModels
{
    public class PedidoViewModel //<T>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome do cliente é obrigatório")]
        public string NomeCliente { get;  set; }

        [EmailAddress(ErrorMessage = "O campo Email é invalido")]
        public string EmailCliente { get; set; }        
        public decimal? ValorTotal { get; set; }
        public bool Pago { get;  set; }
        public List<ItemPedidoViewModel> ItemsPedido { get; set; } = new List<ItemPedidoViewModel>();

        public  PedidoViewModel()
        {

        }
    }
}
