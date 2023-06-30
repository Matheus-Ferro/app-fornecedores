using App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Configurations;

// Configuração do Identity
public static class IdentityConfig
{
	public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, string connectionString)
	{
		// Definição do DbContext padrão do EntityFramework
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));

		services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>();

		return services;
	}
}

