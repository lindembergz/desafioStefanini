using AutoMapper;
using Stefanini.Estoque.App.ViewModel;
using Stefanini.Estoque.Domain.Entities;
using Stefanini.Venda.App.ViewModels;
using Stefanini.Venda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefanini.Estoque.App.AutoMapper
{
    public class ViewModelToDomainPedido : Profile
    {
        public ViewModelToDomainPedido()
        {
            CreateMap<PedidoViewModel, PedidoEntity>()
            .ConstructUsing(p =>
                new PedidoEntity(p.Id, p.NomeCliente, p.EmailCliente, p.Pago));

            CreateMap<ItemPedidoViewModel , ItemPedidoEntity>()
                .ConstructUsing(p =>
                    new ItemPedidoEntity( p.IdPedido,  p.IdProduto,  p.Quantidade, p.ValorUnitario ));

        }
    }
}
