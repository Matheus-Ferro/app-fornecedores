using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels;

/* ViewModels são models exclusivas da camada de apresentação (View), e servem basicamente para controlar um formulário. */
public class EnderecoViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(200, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Logradouro { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(50, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Numero { get; set; }

    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(100, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(8, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Cep { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(100, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(50, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Estado { get; set; }

    [HiddenInput]
    public Fornecedor? Fornecedor { get; set; }

	[HiddenInput]
	public Guid? FornecedorId { get; set; }
}

