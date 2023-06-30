using App.Extensions;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize]
public class ProdutosController : BaseController
{
	private readonly IProdutoRepository _produtoRepository;
	private readonly IProdutoService _produtoService;
	private readonly IFornecedorRepository _fornecedorRepository;
	private readonly IMapper _mapper;

	public ProdutosController(IProdutoRepository produtoRepository,
							  IFornecedorRepository fornecedorRepository,
							  IMapper mapper,
							  IProdutoService produtoService,
							  INotificador notificador) : base(notificador)
	{
		_produtoRepository = produtoRepository;
		_fornecedorRepository = fornecedorRepository;
		_mapper = mapper;
		_produtoService = produtoService;
	}

	// Método que retorna uma View com uma lista de produtos com seus devidos fornecedores
	[Route("lista-de-produtos")]
	[AllowAnonymous]
	public async Task<IActionResult> Index()
	{
		return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
	}

	// Retorna 1 produto com seu devido fornecedor
	[Route("dados-do-produto/{id:guid}")]
	[AllowAnonymous]
	public async Task<IActionResult> Details(Guid id)
	{
		var produtoViewModel = await ObterProduto(id);

		if (produtoViewModel == null) return NotFound();

		return View(produtoViewModel);
	}

	[Route("novo-produto")]
	[ClaimsAuthorize("Produto", "Adicionar")]
	public async Task<IActionResult> Create()
	{
		var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
		return View(produtoViewModel);
	}

	[HttpPost]
	[Route("novo-produto")]
	[ClaimsAuthorize("Produto", "Adicionar")]
	public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
	{
		produtoViewModel = await PopularFornecedores(produtoViewModel);
		if (!ModelState.IsValid) return View(produtoViewModel);

		var imgPrefixo = Guid.NewGuid() + "_";
		if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
		{
			return View(produtoViewModel);
		}

		produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

		await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

		if (!OperacaoValida()) return View(produtoViewModel);

		return RedirectToAction("Index");
	}

	[Route("editar-produto/{id:guid}")]
	[ClaimsAuthorize("Produto", "Editar")]
	public async Task<IActionResult> Edit(Guid id)
	{
		var produtoViewModel = await ObterProduto(id);

		if (produtoViewModel == null) return NotFound();

		return View(produtoViewModel);
	}

	[HttpPost]
	[Route("editar-produto/{id:guid}")]
	[ClaimsAuthorize("Produto", "Editar")]
	public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
	{
		if (id != produtoViewModel.Id) return NotFound();

		var produtoAtualizacao = await ObterProduto(id);
		produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
		produtoViewModel.Imagem = produtoAtualizacao.Imagem;
		if (!ModelState.IsValid) return View(produtoViewModel);

		if (produtoViewModel.ImagemUpload != null)
		{
			var imgPrefixo = Guid.NewGuid() + "_";
			if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
			{
				return View(produtoViewModel);
			}

			produtoAtualizacao.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
		}

		produtoAtualizacao.Nome = produtoViewModel.Nome;
		produtoAtualizacao.Descricao = produtoViewModel.Descricao;
		produtoAtualizacao.Valor = produtoViewModel.Valor;
		produtoAtualizacao.Ativo = produtoViewModel.Ativo;

		await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

		if (!OperacaoValida()) return View(produtoViewModel);

		return RedirectToAction("Index");
	}

	[Route("excluir-produto/{id:guid}")]
	[ClaimsAuthorize("Produto", "Excluir")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var produto = await ObterProduto(id);

		if (produto == null) return NotFound();

		return View(produto);
	}

	[HttpPost, ActionName("Delete")]
	[Route("excluir-produto/{id:guid}")]
	[ClaimsAuthorize("Produto", "Excluir")]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var produto = await ObterProduto(id);
		if (produto == null) return NotFound();

		await _produtoService.Remover(id);

		if (!OperacaoValida()) return View(produto);

        /* Note que quando a operação é um sucesso nós redirecionamos para a action
		 * de Index, ou seja, para que nossa notificação apareça para o usuário em caso
		 * de sucesso precisamos declarar a mensagem dentro de um TempData, pois só ele
		 * sobrevive a um redirecionamento, ao contrario de uma ViewBag. Para entender melhor
		 * veja o arquivo Views/Shared/Components/Summary/Default.cshtml. */
        TempData["Sucesso"] = "Produto excluido com sucesso!";

		return RedirectToAction("Index");
	}

	private async Task<ProdutoViewModel> ObterProduto(Guid id)
	{
		var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));

		// Popula a lista de fornecedores
		produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

		return produto;
	}

	// A partir de qualquer viewModel que você passe, popula os fornecedores da viewModel
	private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
	{
		produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
		return produto;
	}

	private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
	{
		if (arquivo.Length <= 0) return false;

		var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

		if (System.IO.File.Exists(path))
		{
			ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
			return false;
		}

		using (var stream = new FileStream(path, FileMode.Create))
		{
			await arquivo.CopyToAsync(stream);
		}

		return true;
	}
}
