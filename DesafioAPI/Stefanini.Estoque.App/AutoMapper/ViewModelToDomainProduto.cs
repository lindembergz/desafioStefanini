using AutoMapper;
using Stefanini.Estoque.App.ViewModel;
using Stefanini.Estoque.Domain.Entities;

namespace Stefanini.Estoque.App.AutoMapper
{
    public class ViewModelToDomainProduto : Profile
    {
        public ViewModelToDomainProduto()
        {
            CreateMap<ProdutoViewModel, ProdutoEntity>()
                .ConstructUsing(p =>
                    new ProdutoEntity(p.NomeProduto, p.Valor) );

        }
    }
}
