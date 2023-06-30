using App.Extensions;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Authorize]
public class FornecedoresController : BaseController
{
	/* Ao fazer o Scaffolding dos controllers vamos substituir o DbContext pela interface de repositório
     * referente ao controller em questão, nesse caso um repositorio de fornecedor, pois lá já está feita
     * toda a configuração necessária para nos comunicarmos com o DbContext. Vale lembrar que o repositorio
     * só está declarado aqui para fazer as consultas no banco, quem vai chamar o repositorio para fazer
     * a manipulação dos dados é o serviço, que aqui será o FornecedorService.
     * Podemos declarar diretamente um objeto do tipo da interface pois a instanciação já foi feita por meio 
     * de injeção de dependências. O mesmo vale para os serviços, que vão fazer as validações e a manipulação 
     * do repositorio. */
	private readonly IFornecedorRepository _fornecedorRepository;
	private readonly IFornecedorService _fornecedorService;

	// Para trabalharmos com o AutoMapper precisamos declarar uma propriedade com a interface IMapper
	private readonly IMapper _mapper;

	/* Injetamos via construtor as propriedades declaradas acima e a interface de notificação, que é necessária
	 * para que a classe mãe funcione. */
	public FornecedoresController(IFornecedorRepository fornecedorRepository,
								  IMapper mapper,
								  IFornecedorService fornecedorService,
								  INotificador notificador) : base(notificador)
	{
		_fornecedorRepository = fornecedorRepository;
		_mapper = mapper;
		_fornecedorService = fornecedorService;
	}

	[Route("lista-de-fornecedores")]
	[AllowAnonymous]
	public async Task<IActionResult> Index()
	{
		/* Mapeia a lista de fornecedores retornada pelo ObterTodos com destino ao FornecedorViewModel.
         * _mapper.Map<Destino>(Origem).
         * Note que a nossa View que está localizada em Views/Fornecedores/Index.cshtml está esperando um IEnumerable
         * de FornecedorViewModel, portanto vamos mapear o resultado do metodo obter todos, que nos retorna justamente uma lista
         * de fornecedores, para esta view. */
		return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
	}

	// Método que retorna uma view com os detalhes de um fornecedor, incluindo seu endereço
	[Route("dados-do-fornecedor/{id:guid}")]
	[AllowAnonymous]
	public async Task<IActionResult> Details(Guid id)
	{
		var fornecedorViewModel = await ObterFornecedorEndereco(id);
		if (fornecedorViewModel == null)
		{
			return NotFound();
		}

		return View(fornecedorViewModel);
	}

	/* Note que daqui pra baixo teremos sempre dois metodos com o mesmo nome, um que retorna a view e outro
     * que faz o HTTP POST. */
	// View Create
	[Route("novo-fornecedor")]
	[ClaimsAuthorize("Fornecedor", "Adicionar")]
	public IActionResult Create()
	{
		return View();
	}

	// HttpPost Create
	[HttpPost]
	[Route("novo-fornecedor")]
	[ClaimsAuthorize("Fornecedor", "Adicionar")]
	public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
	{
		if (!ModelState.IsValid) return View(fornecedorViewModel);

		var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
		await _fornecedorService.Adicionar(fornecedor);

		// Verifica com base nas notificações se a operação é valida ou não, caso não seja valida
		// retorna a View novamente
		if (!OperacaoValida()) return View(fornecedorViewModel);

		return RedirectToAction("Index");
	}

	// View Edit
	[Route("editar-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Editar")]
	public async Task<IActionResult> Edit(Guid id)
	{
		var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);
		if (fornecedorViewModel == null) return NotFound();

		return View(fornecedorViewModel);
	}

	// HttpPost Edit
	[HttpPost]
	[Route("editar-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Editar")]
	public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
	{
		if (id != fornecedorViewModel.Id) return NotFound();

		if (!ModelState.IsValid) return View(fornecedorViewModel);

		var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
		await _fornecedorService.Atualizar(fornecedor);

		if (!OperacaoValida()) return View(await ObterFornecedorProdutosEndereco(id));

		return RedirectToAction("Index");


	}

	// View Delete
	[Route("excluir-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Excluir")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var fornecedorViewModel = await ObterFornecedorEndereco(id);

		if (fornecedorViewModel == null) return NotFound();

		return View(fornecedorViewModel);
	}

	// HttpPost Delete
	/* Um detalhe que vale a pena ser lembrado é que, diferente dos outros metodos, que possuem
	 * o par View/Post, o delete não pode ter o mesmo nome da View, pois a assinatura de ambos 
	 * seria a mesma, ambos tem o mesmo nome e recebem os mesmos parametros, o que faz com que sejam
	 * identicos para o compilador, diferente dos outros metodos que possuem o mesmo nome mas que recebem
	 * parametros diferentes, para contornar isso mudamos o nome do metodo e especificamos que a ActionName
	 * dele será Delete, assim mesmo que ele tenha outro nome responderá como Delete. */
	[HttpPost, ActionName("Delete")]
	[Route("excluir-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Excluir")]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var fornecedor = await ObterFornecedorEndereco(id);

		if (fornecedor == null) return NotFound();

		await _fornecedorService.Remover(id);

		if (!OperacaoValida()) return View(fornecedor);

		return RedirectToAction("Index");
	}

	// Action que retorna a view atualizada do endereço para a variavel "url" da Action AtualizarEndereco
	[Route("obter-endereco-fornecedor/{id:guid}")]
	[AllowAnonymous]
	public async Task<IActionResult> ObterEndereco(Guid id)
	{
		var fornecedor = await ObterFornecedorEndereco(id);
		if (fornecedor == null) return NotFound();
		return PartialView("_DetalhesEndereco", fornecedor);
	}

	// View AtualizarEndereco
	[Route("atualizar-endereco-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Editar")]
	public async Task<IActionResult> AtualizarEndereco(Guid id)
	{
		var fornecedor = await ObterFornecedorEndereco(id);
		if (fornecedor == null) return NotFound();
		return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
	}

	// HttpPost que atualiza o endereço
	[HttpPost]
	[Route("atualizar-endereco-fornecedor/{id:guid}")]
	[ClaimsAuthorize("Fornecedor", "Editar")]
	public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
	{
		// Nome e Documento não são passados para a model, portanto precisamos remove-los da ModelState 
		ModelState.Remove("Nome");
		ModelState.Remove("Documento");

		if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

		// Mapeia o endereço da fornecedorViewModel para a Model de Endereco, assim podemos persistir na tabela de endereços
		await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

		if (!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorViewModel);

		// Metodo Action da interface IUrlHelper, que nos retorna uma string com uma URL com o caminho para uma action
		var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });

		// Retorna um json com as propriedades de success e url que serão utilizadas no JS para exibir o endereço atualizado
		return Json(new { success = true, url });
	}

	// Método que retorna uma ViewModel de um fornecedor com o endereço dele
	public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
	{
		return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
	}

	// Método que retorna a lista de produtos de um fornecedor
	public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
	{
		return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
	}
}
