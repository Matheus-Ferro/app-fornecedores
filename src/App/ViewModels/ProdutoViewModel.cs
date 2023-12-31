﻿using App.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.ViewModels;

public class ProdutoViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [DisplayName("Fornecedor")]
    public Guid FornecedorId { get; set; }

    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(200, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Nome { get; set; }

    [DisplayName("Descrição")]
    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    [StringLength(1000, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Descricao { get; set; }

    [DisplayName("Imagem do Produto")]
    public IFormFile? ImagemUpload { get; set; }
    public string? Imagem { get; set; }

    [Moeda]
    [Required(ErrorMessage = "O Campo {0} é obrigatorio")]
    public decimal Valor { get; set; }

    [ScaffoldColumn(false)]
    public DateTime DataCadastro { get; set; }

    [DisplayName("Ativo?")]
    public bool Ativo { get; set; }

    public FornecedorViewModel? Fornecedor { get; set; }

    public IEnumerable<FornecedorViewModel>? Fornecedores { get; set; }
}

