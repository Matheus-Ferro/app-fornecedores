using Business.Interfaces;
using Business.Models;
using Business.Models.Validations;

namespace Business.Services;

/* Qual a ideia por tras dessas classes de serviços?
 * Antes de criar estas classes de serviço as validações eram feitas na Controller, o que não é legal, sem falar que
 * a view tinha acesso direto ao repositório*/
public class FornecedorService : BaseService, IFornecedorService
{
	private readonly IFornecedorRepository _fornecedorRepository;
	private readonly IEnderecoRepository _enderecoRepository;

	public FornecedorService(IFornecedorRepository fornecedorRepository,
							 IEnderecoRepository enderecoRepository,
							 INotificador notificador) : base(notificador)
	{
		_fornecedorRepository = fornecedorRepository;
		_enderecoRepository = enderecoRepository;
	}

	public async Task Adicionar(Fornecedor fornecedor)
	{
		// Se a validação do fornecedor ou do endereço for falsa, vai entrar na condição e retornar sem persistir o fornecedor
		if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
			|| !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

		// Verifica se já existe um fornecedor com este documento, se já existir retorna sem persistir o fornecedor
		if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
		{
			Notificar("Já existe um fornecedor com este documento informado.");
			return;
		}

		// Caso não tenha retornado em nenhuma das condições acima, por fim, persiste o fornecedor
		await _fornecedorRepository.Adicionar(fornecedor);
	}

	public async Task Atualizar(Fornecedor fornecedor)
	{
		if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

		if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
		{
			Notificar("Já existe um fornecedor com este documento informado.");
			return;
		}

		await _fornecedorRepository.Atualizar(fornecedor);
	}

	public async Task AtualizarEndereco(Endereco endereco)
	{
		if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

		await _enderecoRepository.Atualizar(endereco);
	}

	public async Task Remover(Guid id)
	{
		if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
		{
			Notificar("O fornecedor possui produtos cadastrados.");
			return;
		}

		await _fornecedorRepository.Remover(id);
	}
	public void Dispose()
	{
		_fornecedorRepository?.Dispose();
		_enderecoRepository?.Dispose();
	}
}

