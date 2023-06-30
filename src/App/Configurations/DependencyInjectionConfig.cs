using Business.Interfaces;
using Business.Notificacoes;
using Business.Services;
using Data.Context;
using Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using static App.Extensions.MoedaAttribute;

namespace App.Configurations;

// Classe responsável por fazer todas as injeções de dependência da aplicação
public static class DependencyInjectionConfig
{
	public static IServiceCollection ResolveDependencies(this IServiceCollection services)
	{
		/* Lembrando sobre o ciclo de vida das resoluções de dependencias
		 * SCOPED - cria uma alocação de memoria unica por cada request, e que não muda de instancia para
		 * instancia, ou seja, permanece o mesmo por todo o request.
		 * SINGLETON - cria uma alocação de memoria unica com valores unicos, não muda de instancia para
		 * instancia, inclusive entre browsers */
		services.AddScoped<AppFornecedoresDbContext>();
		services.AddScoped<IProdutoRepository, ProdutoRepository>();
		services.AddScoped<IFornecedorRepository, FornecedorRepository>();
		services.AddScoped<IEnderecoRepository, EnderecoRepository>();
		services.AddSingleton<IValidationAttributeAdapterProvider, MoedaAttributeAdapterProvider>();

		services.AddScoped<INotificador, Notificador>();
		services.AddScoped<IFornecedorService, FornecedorService>();
		services.AddScoped<IProdutoService, ProdutoService>();

		return services;
	}
}
