using Business.Models;

namespace Business.Interfaces;

// Interface de serviço para adição de Produtos
public interface IProdutoService : IDisposable
{
	Task Adicionar(Produto produto);
	Task Atualizar(Produto produto);
	Task Remover(Guid id);
}

