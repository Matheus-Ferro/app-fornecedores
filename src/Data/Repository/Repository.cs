using Business.Models;
using Business.Interfaces;
using System.Linq.Expressions;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

/* Esta é uma classe abstrata genérica que será herdada por outros repositórios mais específicos, ela serve
 * basicamente para implementar a interface também genérica, IRepository, com todas as suas tasks.
 * Observe que no final da declaração desta classe utilizamos new(), isso serve pra informar que podemos
 * instanciar a o tipo genérico TEntity, vamos precisar disso para remover um objeto do banco. */
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    // Essa classe será herdada por outras classes, e para que todas tenham acesso ao Db e o DbSet vamos
    // declara-los aqui.
    protected readonly AppFornecedoresDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(AppFornecedoresDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        /* Aqui vamos chamar o método AsNoTracking() para que o objeto não fique no "tracking", que é onde
         * o .NET observa a memória constantemente para verificar mudanças de estado, então para obter mais
         * performance vamos utilizar o AsNoTracking(). Vale lembrar que este metodo desconecta entidades do
         * banco de dados, portanto não é útil para manipulação do banco, mas indispensavel para realizar consultas. */
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity> ObterPorId(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> ObterTodos()
    {
		/* Aqui obtive o seguinte erro: "The instance of the entity type cannot be tracked because another instance 
		 * with the keyvalue is being tracked". Isso se deu pois esqueci de realizar a consulta com o AsNoTracking(),
         * caso não seja informado o EF vai rastrear o resultado, e se tentar fazer Update na entidade vai
         * retornar o erro, já que ele está tentando rastrear duas entidades com a mesma PK, portanto consulte com
         * AsNoTracking(), e rastreie somente a entidade alterada. */
		return await DbSet.AsNoTracking().ToListAsync();
    }
    public virtual async Task Adicionar(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChanges();
    }

    public virtual async Task Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChanges();
    }

    public virtual async Task Remover(Guid id)
    {
        /* O método Remove() do DbSet espera receber uma entidade para ser deletada, mas só temos o id
         * para contornar este problema vamos instanciar a interface TEntity de forma genérica e passar o 
         * id do objeto a ser removido para a propriedade Id da interface.
         * Basicamente vamo criar uma instância da entidade que tenha esse Id e deletar ela do banco. 
         * Mas como isso é possível? Por todo mundo herdar de entity, e entity definir o id, todos tem um id único, logo
         * é só criar essa instância na memória e para poder deleta-la do banco. Essa é a vantagem de montar um objeto de forma genérica. */
        DbSet.Remove(new TEntity { Id = id });
        await SaveChanges();
    }

    public async Task<int> SaveChanges()
    {
        // Agora você deve estar se perguntando, não era só chamar Db.SaveChangesAsync() diretamente nos outros métodos para salvar os dados?
        // Sim, seria possível, mas para termos um código limpo, e para facilitar a nossa vida ao ter de fazer um tratamento específico do
        // SaveChanges, ou um try-catch, vamos fazer em um único método, e não em cada método que chama o SaveChanges.
        return await Db.SaveChangesAsync();
    }
    public void Dispose()
    {
        Db?.Dispose();
    }
}

