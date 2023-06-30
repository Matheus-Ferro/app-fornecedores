using Business.Interfaces;

namespace Business.Notificacoes;

// Esta classe é quem gerencia as notificações, e será instanciada por meio de injeção de dependencias
public class Notificador : INotificador
{
	// Lista de notificações, inicializada no construtor, reseta a cada request
	private List<Notificacao> _notificacoes;

	public Notificador()
	{
		_notificacoes = new List<Notificacao>();
	}

	// Adiciona notificações na lista de notificação
	public void Handle(Notificacao notificacao)
	{
		_notificacoes.Add(notificacao);
	}

	// Metodo criado só pra retornar a lista de notificações
	public List<Notificacao> ObterNotificacoes()
	{
		return _notificacoes;
	}

	// Metodo criado pra dizer se tem ou não notificações na lista
	public bool TemNotificacao()
	{
		return _notificacoes.Any();
	}
}

