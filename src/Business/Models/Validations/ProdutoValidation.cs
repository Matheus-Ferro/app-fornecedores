﻿using FluentValidation;

namespace Business.Models.Validations;

// Regras de validação para os campos de produtos, para saber mais sobre a biblioteca consulte o arquivo FornecedorValidation.cs
public class ProdutoValidation : AbstractValidator<Produto>
{
	public ProdutoValidation()
	{
		RuleFor(c => c.Nome)
			.NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
			.Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

		RuleFor(c => c.Descricao)
			.NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
			.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

		RuleFor(c => c.Valor)
			.GreaterThan(0).WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}");
	}
}