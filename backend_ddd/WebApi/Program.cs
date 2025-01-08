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

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();

// Configura o Swagger para documentação da API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o contexto do banco de dados com a conexão definida.
builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura as interfaces e repositórios.
builder.Services.AddScoped(typeof(InterfaceGeneric<>), typeof(RepositorioGeneric<>));
builder.Services.AddScoped(typeof(RepositoryBase<>));  // Mantém o genérico
builder.Services.AddScoped<RepositoryBase<TaskEntity>>();

// Configuração de CORS para permitir acesso de diferentes origens
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
