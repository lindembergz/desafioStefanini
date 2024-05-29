using Stefanini.Estoque.App.ViewModel;

namespace Stefanini.Estoque.App.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoViewModel> GetById(int idProduto);
        Task CreateProduto(ProdutoViewModel produto);
    }
}
