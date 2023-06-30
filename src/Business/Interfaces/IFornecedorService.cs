using Business.Models;

namespace Business.Interfaces;

// Interface de serviço para adição de fornecedores
public interface IFornecedorService : IDisposable
{
	Task Adicionar(Fornecedor fornecedor);
	Task Atualizar(Fornecedor fornecedor);
	Task Remover(Guid id);
	Task AtualizarEndereco(Endereco endereco);
}

