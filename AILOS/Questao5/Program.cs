using MediatR;
using Microsoft.EntityFrameworkCore;
using Questao5.Application.Validators;
using Questao5.Infrastructure.Context;
using Questao5.Infrastructure.Sqlite;
using Questao5.Setup;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
var databaseConfig = new DatabaseConfig { 
                     Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") 
                     };

builder.Services.AddDbContext<SQLiteContext>(options => { options.UseSqlite(databaseConfig.Name); });
builder.Services.AddSingleton(databaseConfig);
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

DependencyInjection.RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapGet("/", () => { return Results.Redirect("/swagger/index.html"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.


app.Run();



