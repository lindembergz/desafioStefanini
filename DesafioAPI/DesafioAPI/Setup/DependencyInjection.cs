using Stefanini.Estoque.App.AutoMapper;

using Stefanini.Estoque.App.Services.Implementation;
using Stefanini.Estoque.App.Services.Interfaces;

using Stefanini.Estoque.Data.Repositories.Implementation;
using Stefanini.Estoque.Domain.Repository.Interfaces;

using Stefanini.Venda.App.Services.Implementation;
using Stefanini.Venda.App.Services.Interfaces;

using Stefanini.Venda.Data.Repositories.Implementation;
using Stefanini.Venda.Domain.Repository.Interfaces;

namespace Stefanini.Api.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddAutoMapper(typeof(DomainToViewModelProduto), typeof(ViewModelToDomainProduto));
            services.AddAutoMapper(typeof(DomainToViewModelPedido), typeof(ViewModelToDomainPedido));
         }
    }
}

