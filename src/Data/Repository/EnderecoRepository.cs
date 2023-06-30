using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

/* Estas classes de repositório servem para implementar as especificidades de cada model. */
public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(AppFornecedoresDbContext context) : base(context) { }

    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await Db.Enderecos.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == fornecedorId);
    }
}

