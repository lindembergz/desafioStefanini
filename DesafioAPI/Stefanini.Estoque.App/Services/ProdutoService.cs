using AutoMapper;
using Stefanini.Estoque.App.ViewModel;
using Stefanini.Estoque.Domain.Entities;
using Stefanini.Estoque.App.Services.Interfaces;
using Stefanini.Estoque.Domain.Repository.Interfaces;

namespace Stefanini.Estoque.App.Services.Implementation
{
    public class ProdutoService : IProdutoService
    {

        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        protected ProdutoService()
        {
        }

        public ProdutoService(IMapper mapper, IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }
        public async Task<ProdutoViewModel> GetById(int id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.GetById(id));
        }

        public async Task CreateProduto(ProdutoViewModel produto)
        {
            var _produto = _mapper.Map<ProdutoEntity>(produto);
            await _produtoRepository.Insert(_produto);
        }
    }
}
