// Stefanini.Venda.Domain/Interfaces/IProdutoRepository.cs
using Stefanini.Estoque.Domain;
using Stefanini.Estoque.Domain.Entities;

namespace Stefanini.Estoque.Domain.Repository.Interfaces
{
    public interface IProdutoRepository
    {
        Task<ProdutoEntity> GetById(int id);
        Task Insert(ProdutoEntity produto);
    }
}