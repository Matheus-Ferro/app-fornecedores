using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;
public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
	public ProdutoRepository(AppFornecedoresDbContext context) : base(context) { }

	public async Task<Produto> ObterProdutoFornecedor(Guid id)
	{
		// Retorna 1 produto pelo fornecedor
		return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor).FirstOrDefaultAsync(p => p.Id == id);
	}

	public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
	{
		// Retorna uma lista de produtos
		return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor).OrderBy(p => p.Nome).ToListAsync();
	}

	public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
	{
		return await Buscar(p => p.FornecedorId == fornecedorId);
	}
}

