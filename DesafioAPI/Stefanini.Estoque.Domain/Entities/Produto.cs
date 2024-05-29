using Stefanini.Core.DomainObjects;

namespace Stefanini.Estoque.Domain.Entities
{
    public class ProdutoEntity : Entity<int>
    {
        public string NomeProduto { get; private set; }
        public decimal Valor { get; private set; }
        protected ProdutoEntity() { }
        public ProdutoEntity(string nomeProduto, decimal valor)
        {
            NomeProduto = nomeProduto;
            Valor = valor;
            Validar();
        }
        public void Validar()
        {
            Validation.ValidarSeVazio(NomeProduto, "O campo Nome do produto não pode estar vazio");
        }
    }
}