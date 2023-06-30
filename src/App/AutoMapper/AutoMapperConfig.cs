using App.ViewModels;
using AutoMapper;
using Business.Models;

namespace App.AutoMapper;

/* Aqui serão declaradas as configurações do AutoMapper, que serve para mapear um objeto
 * para outro, neste caso entre a Model e a ViewModel e vice-versa, por exemplo, ao invés
 * de instanciarmos as duas model, instanciaremos uma só e mapearemos para a outra somente
 * o que precisarmos nela.
 * Vale lembrar que o mapeamento é feito de forma automática se o nome das propriedades e métodos
 * forem iguais tanto na origem quanto no destino. Caso o nome seja diferente consulte a documentação
 * pois precisa de um tratamento diferente aqui no perfil. */
public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
        CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
        CreateMap<Produto, ProdutoViewModel>().ReverseMap();
    }
}

