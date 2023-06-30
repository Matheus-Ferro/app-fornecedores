namespace Business.Models;

// Esta classe foi criada só para gerar os Ids das entidades
public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
}

