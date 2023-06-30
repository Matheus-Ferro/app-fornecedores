using Microsoft.AspNetCore.Razor.TagHelpers;

namespace App.Extensions;

/* Aqui passamos a tag, caso queira que esteja disponivel para todas as tags utilizamos *
 * depois passamos qual vai ser o nome do atributo */
[HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
[HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
public class ApagaElementoByClaimTagHelper : TagHelper
{
    /* A interface IHttpContextAccessor é um meio de se acessar o contexto via Http, ela será necessaria pois
     * para validar as claims do usuário vamos precisar do HttpContext, porém o contexto que recebemos atraves do Process 
     * não é um contexto completo, por isso injetamos este contexto */
    private readonly IHttpContextAccessor _contextAccessor;

    public ApagaElementoByClaimTagHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    [HtmlAttributeName("supress-by-claim-name")]
    public string IdentityClaimName { get; set; }

    [HtmlAttributeName("supress-by-claim-value")]
    public string IdentityClaimValue { get; set; }

    // Tudo o que for relacionado à manipulação da TagHelper deve ser feito dentro deste metodo
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // O contexto é o que estamos recebendo atraves da taghelper e o output é a saída que a tag helper está produzindo
        if (context == null) throw new ArgumentNullException("context");
        if (output == null) throw new ArgumentNullException("output");

        /* Variavel que guarda o resultado do metodo ValidarClaimsUsuario, que retorna true se o usuario estiver autenticado 
         * e tiver as claims necessarias, ou false caso contrario. */
        var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

        // Caso a variavel seja verdadeira nada será feito, e a tag será renderizada normalmente
        if (temAcesso) return;

        // Caso a variavel seja falsa chegamos aqui e a tag nem será renderizada
        output.SuppressOutput();
    } 
}


[HtmlTargetElement("a", Attributes = "disable-by-claim-name")]
[HtmlTargetElement("a", Attributes = "disable-by-claim-value")]
public class DesabilitaLinkByClaimTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public DesabilitaLinkByClaimTagHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    [HtmlAttributeName("disable-by-claim-name")]
    public string IdentityClaimName { get; set; }

    [HtmlAttributeName("disable-by-claim-value")]
    public string IdentityClaimValue { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context == null) throw new ArgumentNullException("context");
        if (output == null) throw new ArgumentNullException("output");

        var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

        if (temAcesso) return;

		// Caso a variavel seja falsa chegamos aqui
		output.Attributes.RemoveAll("href"); // O href do link será removido
        output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed")); // Adiciona um estilo
        output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão")); // Adiciona um titulo
    }
}

[HtmlTargetElement("*", Attributes = "supress-by-action")]
public class ApagaElementoByActionTagHelper : TagHelper
{
	private readonly IHttpContextAccessor _contextAccessor;

	public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	[HtmlAttributeName("supress-by-action")]
	public string ActionName { get; set; }

	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		if (context == null) throw new ArgumentNullException("context");
		if (output == null) throw new ArgumentNullException("output");

        // Pega os valores contidos na chave action do HttpContext e transforma em string
		var action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

        // Se conter o valor da action nada acontece
		if (ActionName.Contains(action)) return;

        // Caso não contenha chegamos aqui e suprimimos o elemento
		output.SuppressOutput();
	}
}
