using Stefanini.Estoque.Data.Context;
using Stefanini.Estoque.Domain.Entities;
using Stefanini.Estoque.Domain.Repository.Interfaces;

namespace Stefanini.Estoque.Data.Repositories.Implementation
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly EstoqueContext _context;

        protected ProdutoRepository()
        {
        }

        public ProdutoRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task<ProdutoEntity> GetById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
             return produto;
        }


        public async Task Insert(ProdutoEntity produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }
    }
}