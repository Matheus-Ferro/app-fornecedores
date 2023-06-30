using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace App.Configurations;

// Classe responsável pela globalização da aplicação
public static class GlobalizationConfig
{
	public static IApplicationBuilder UseGlobalizationConfig(this IApplicationBuilder app)
	{
		var defaultCulture = new CultureInfo("pt-BR");
		var localizationOptions = new RequestLocalizationOptions
		{
			DefaultRequestCulture = new RequestCulture(defaultCulture),
			SupportedCultures = new List<CultureInfo> { defaultCulture, new CultureInfo("en-US") },
			SupportedUICultures = new List<CultureInfo> { defaultCulture, new CultureInfo("en-US") }
		};
		app.UseRequestLocalization(localizationOptions);

		return app;
	}
}

