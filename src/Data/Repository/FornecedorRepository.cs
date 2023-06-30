﻿using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;
public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
	public FornecedorRepository(AppFornecedoresDbContext context) : base(context) { }

	public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
	{
		return await Db.Fornecedores
			.AsNoTracking()
			.Include(e => e.Endereco)
			.FirstOrDefaultAsync(c => c.Id == id);
	}

	public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
	{
		return await Db.Fornecedores
			.AsNoTracking()
			.Include(c => c.Produtos)
			.Include(c => c.Endereco)
			.FirstOrDefaultAsync(c => c.Id == id);
	}
}

