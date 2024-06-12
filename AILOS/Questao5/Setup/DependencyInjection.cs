using FluentValidation;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Services;
using Questao5.Application.Validators;
using Questao5.Core.Data;
using Questao5.Domain.Interfaces.Repositories;
using Questao5.Domain.Interfaces.Services;
using Questao5.Infrastructure.Context;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, SQLiteContext>();

            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

            services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
            services.AddScoped<IMovimentoService, MovimentoService>();

        }
    }
}
