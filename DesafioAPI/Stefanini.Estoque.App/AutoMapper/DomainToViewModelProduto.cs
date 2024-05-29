using Stefanini.Estoque.App.ViewModel;
using AutoMapper;
using Stefanini.Estoque.Domain.Entities;

namespace Stefanini.Estoque.App.AutoMapper
{
    public class DomainToViewModelProduto : Profile
    {
        public DomainToViewModelProduto()
        {
            CreateMap<ProdutoEntity, ProdutoViewModel>();

        }
    }
}
