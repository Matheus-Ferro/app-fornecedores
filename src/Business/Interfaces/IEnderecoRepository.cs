using Business.Models;

namespace Business.Interfaces;

/* Estas interfaces do business serão utilizadas na camada de dados, já que a camada de Business não conhece Data
 * mas Data conhece Business. */
public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
}

