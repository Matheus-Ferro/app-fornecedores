using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Extensions;

// Classe para validar as Claims
public class CustomAuthorization
{
	public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
	{
		// Verifica se o usuario esta autenticado e se possui a claim com o valor necessario
		return context.User.Identity.IsAuthenticated &&
			   context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
	}

}

/* Classe para fazer com que o filtro seja usado como atributo, por isso herda de TypeFilterAttribute.
 * Vale lembrar que ao invocar esta classe precisamos passar somente o ClaimsAuthorize, devemos ocultar
 * Attribute do nome */
public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    /* Observe que ao invocar este atributo precisamos passar o nome e o valor da Claim, dessa forma vamos inicializar
     * a classe RequisitoClaimFilter por meio da classe base, e então ela vai fazer toda a validação necessária. */
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
	{
		Arguments = new object[] { new Claim(claimName, claimValue) };
	}
}

// Filtro para confirmar ou negar o request
public class RequisitoClaimFilter : IAuthorizationFilter
{
	private readonly Claim _claim;

	public RequisitoClaimFilter(Claim claim)
	{
		_claim = claim;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		// Se o usuário não estiver autenticado, o leva para a página de login
		if (!context.HttpContext.User.Identity.IsAuthenticated)
		{
			context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
			return;
		}

		// Se as claims do usuário não baterem, o leva para a página de acesso negado, cujo código é 403
		if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
		{
			context.Result = new StatusCodeResult(403);
		}
	}
}


