using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Extensions;

// Classe que resume os erros e os exibe na view diretamente
public class SummaryViewComponent : ViewComponent
{
	private readonly INotificador _notificador;

	public SummaryViewComponent(INotificador notificador)
	{
		_notificador = notificador;
	}

	/* Por herdar de ViewComponent podemos chamar este metodo, que executa uma tarefa de forma assincrona,
	 * vamos utiliza-lo para retornar a View Deafult que está declarada em Views/Shared/Components/Summary/Default.cshtml
	 * Note que este é o nome padrão para este tipo de View, e que o caminho também é um caminho padrão para declarar
	 * este tipo de ViewComponent */
	public async Task<IViewComponentResult> InvokeAsync()
	{
		/* Como o metodo ObterNotificacoes não é assincrono, e estamos dentro de um contexto assincrono, precisamos utilizar
		 * o await Task.FromResult() e o método que precisamos do resultado. */
		var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

		/* Para cada notificação vamos adicionar dentro da ModelState um ModelError, cujo a chave vai ser vazia e o valor
		 * vai ser a mensagem de erro, assim podemos declarar a ModelState como invalida caso algum erro ocorra, e vamos
		 * compreender como um erro de preeenchimento de campo, só que sem um campo especifico. */
		notificacoes.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Mensagem));

		return View();
	}
}
