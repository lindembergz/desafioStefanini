using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefanini.Venda.App.ViewModels
{
    public class ItemPedidoViewModel//<T> 
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public string? NomeProduto { get; set; }        
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }

    }
}
