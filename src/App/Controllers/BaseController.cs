using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

// Todas as controllers herdam desta classe, aqui ficará tudo o que for comum a elas
public class BaseController : Controller
{
	private readonly INotificador _notificador;

	public BaseController(INotificador notificador)
	{
		_notificador = notificador;
	}

	// Invoca o metodo TemNotificação, se tiver a operacao é invalida (false)
	// se não tiver notificações a operação será valida (true).
	protected bool OperacaoValida()
	{
		return !_notificador.TemNotificacao();
	}
}
