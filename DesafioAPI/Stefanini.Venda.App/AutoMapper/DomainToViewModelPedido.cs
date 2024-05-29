
using AutoMapper;
using Stefanini.Venda.Domain.Entities;
using Stefanini.Venda.App.ViewModels;

namespace Stefanini.Estoque.App.AutoMapper
{
    public class DomainToViewModelPedido : Profile
    {
        public DomainToViewModelPedido()
        {
            CreateMap<PedidoEntity, PedidoViewModel>()
            .ForMember(dest => dest.ItemsPedido, opt => opt.MapFrom(src => src.PedidoItems));

            CreateMap<ItemPedidoEntity, ItemPedidoViewModel>()
                .ForMember(d => d.NomeProduto, prd => prd.MapFrom(src => src.Produto.NomeProduto));

        }
    }
}
