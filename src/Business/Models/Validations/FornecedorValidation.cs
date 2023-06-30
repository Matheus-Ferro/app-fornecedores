using Business.Models.Validations.Documentos;
using FluentValidation;

namespace Business.Models.Validations;

/* Aqui instalamos o FluentValidation, uma biblioteca para nos auxiliar a fazer a validação dos campos
 * da view.
 * Observe que esta classe está herdando de AbstractValidator, assim podemos utilizar os métodos do FluentValidation.
 * Esta classe será instanciada em FornecedorService. */
public class FornecedorValidation : AbstractValidator<Fornecedor>
{
	// As regras do FluentValidation são definidas dentro do construtor.
	public FornecedorValidation()
	{
		// Regra para validar o campo do nome do fornecedor
		RuleFor(f => f.Nome)
			.NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido!")
			.Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres!");

		// Executa quando o tipo do fornecedor for pessoa física
		When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
		{
			/* Para validar estes documentos foi necessário criar uma outra classe com uma série de métodos, 
			 * para conferi-los basta verificar o arquivo ValidacaoDocs.cs na pasta Documentos */
			RuleFor(f => f.Documento.Length)
				.Equal(CpfValidacao.TamanhoCpf).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}!");

			RuleFor(f => CpfValidacao.Validar(f.Documento))
				.Equal(true).WithMessage("O documento fornecido é inválido!");
		});

		// Executa quando o tipo do fornecedor for pessoa juridica
		When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
		{
			/* Para validar estes documentos foi necessário criar uma outra classe com uma série de métodos, 
			 * para conferi-los basta verificar o arquivo ValidacaoDocs.cs na pasta Documentos */
			RuleFor(f => f.Documento.Length)
				.Equal(CnpjValidacao.TamanhoCnpj).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}!");

			RuleFor(f => CnpjValidacao.Validar(f.Documento))
				.Equal(true).WithMessage("O documento fornecido é inválido!");
		});
	}
}
