using Business.Notificacoes;

namespace Business.Interfaces;

// Interface utilizada para propagar os erros na view
public interface INotificador
{
	bool TemNotificacao(); // Verifica se tem notificações
	List<Notificacao> ObterNotificacoes(); // Lista de notificações
	void Handle(Notificacao notificacao); // Manuseio das notificações
}

