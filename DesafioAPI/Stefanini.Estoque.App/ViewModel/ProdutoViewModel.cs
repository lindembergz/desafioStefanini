
namespace Stefanini.Estoque.App.ViewModel
{

    public class ProdutoViewModel 
    {
        public int Id { get; set; }
        public string NomeProduto { get;  set; }
        public decimal Valor { get;  set; }
        public ProdutoViewModel( string nomeProduto, decimal valor) { }

    }
}
