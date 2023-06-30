using Business.Interfaces;
using Business.Models;
using Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Services;

/* Classe abstrata que contém tudo o que é comum às outras classes de serviço
 * Aqui configuraremos basicamente como as notificações de erro funcionarão */
public abstract class BaseService
{
	private readonly INotificador _notificador;

	protected BaseService(INotificador notificador)
	{
		_notificador = notificador;
	}

	/* Aqui criamos dois métodos com o nome Notificar, onde um recebe uma coleção de
	 * erros, por meio do tipo ValidationResult do FluentValidation, e o outro metodo
	 * recebe somente uma string com a mensagem. Note que quem chama este método é o
	 * ExecutarValidacao(), declarado no fim deste arquivo. */
	protected void Notificar(ValidationResult validationResult)
	{
		// Para cada erro dentro da lista
		foreach (var error in validationResult.Errors)
		{
			// Chama o metodo responsavel por propagar a mensagem
			Notificar(error.ErrorMessage);
		}
	}

	// Metodo responsavel por propagar o erro até a camada de apresentação
	protected void Notificar(string mensagem)
	{
		_notificador.Handle(new Notificacao(mensagem));
	}

	// Metodo responsavel por executar a validação, recebe a classe com as regras de validação e a entidade a ser validada
	// onde a classe de validação deve herdar de AbstractValidator e a entidade deve herdar de Entity.
	protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
	{
		var validator = validacao.Validate(entidade);

		if (validator.IsValid) return true;

		Notificar(validator);

		return false;
	}
}

