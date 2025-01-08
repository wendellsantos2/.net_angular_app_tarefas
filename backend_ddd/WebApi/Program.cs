using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infra.repository.generics;
using Infra.repository;
using dominio.Interfaces.Generics;
using dominio.Interfaces;
using Entities.Context;
using Entities.Entidades;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllers();

// Configura o Swagger para documenta��o da API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o contexto do banco de dados com a conex�o definida.
builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura as interfaces e reposit�rios.
builder.Services.AddScoped(typeof(InterfaceGeneric<>), typeof(RepositorioGeneric<>));
builder.Services.AddScoped(typeof(RepositoryBase<>));  // Mant�m o gen�rico
builder.Services.AddScoped<RepositoryBase<TaskEntity>>();

// Configura o CORS para permitir solicita��es do frontend Angular.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:4200") // Substitua pelo endere�o do seu frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica a pol�tica de CORS definida.
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
