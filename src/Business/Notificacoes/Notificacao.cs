namespace Business.Notificacoes;

/* Classe da mensagem de notificação, ela só existe para guardar a
 * mensagem e repassar quando necessario */
public class Notificacao
{
	public string Mensagem { get; }

	/* A mensagem é recebida na instanciação da classe e então é armazenada
	 * na propriedade Mensagem */
	public Notificacao(string mensagem)
	{
		Mensagem = mensagem;
	}

}