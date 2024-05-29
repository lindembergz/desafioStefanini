using Microsoft.EntityFrameworkCore;
using Stefanini.Venda.Data.Context;
using Stefanini.Estoque.Data.Context;
using Stefanini.Api.Setup;
using DesafioAPI.Api.Pedido.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var conectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>{ options.UseSqlite(conectionString);});
builder.Services.AddDbContext<VendaContext>(options =>{options.UseSqlite(conectionString);});
builder.Services.AddDbContext<EstoqueContext>(options =>{options.UseSqlite(conectionString);});

DependencyInjection.RegisterServices(builder.Services);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () =>{return Results.Redirect("/swagger/index.html");});
}

app.UseHttpsRedirection();

PedidoController.Configure(app);

app.Run();





