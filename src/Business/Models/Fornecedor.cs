namespace Business.Models;

// Model de um fornecedor que será usada no DB
public class Fornecedor : Entity
{
    public string Nome { get; set; }
    public string Documento { get; set; }
    public TipoFornecedor TipoFornecedor { get; set; }
    public Endereco Endereco { get; set; }
    public bool Ativo { get; set; }
    public IEnumerable<Produto> Produtos { get; set; }
}

