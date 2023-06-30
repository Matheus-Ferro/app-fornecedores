using Business.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

/* Interface genérica que implementa tasks comuns a todos os models. Ela recebe um tipo genérico que nomeamos de TEntity, 
 * e que só aceita tipos que herdam da classe Entity. Essa interface também implementa IDisposable para obrigar que o 
 * repositório faça o Dispose para liberar memória. */
public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    // Todos os métodos deste repositório são assíncronos para garantir a melhor performance da aplicação e melhor saúde do servidor.
    // Vale lembrar que entre os sinais <> é o que a Task vai retornar, se nada for informado retorna void, ou seja, é só pra execução.
    Task Adicionar(TEntity entity);
    Task<TEntity> ObterPorId(Guid id);
    Task<List<TEntity>> ObterTodos();
    Task Atualizar(TEntity entity);
    Task Remover(Guid id);

    // Possibilita que se passe uma expressão Lambda (=>) dentro desse método para buscar qualquer entidade por qualquer parâmetro,
    // desde que seja retornado um boolean, o resultado foi apelidade de predicate
    Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);

    Task<int> SaveChanges();
}

