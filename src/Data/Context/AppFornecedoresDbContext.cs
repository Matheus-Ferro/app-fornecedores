using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppFornecedoresDbContext : DbContext
{
    public AppFornecedoresDbContext(DbContextOptions options) : base(options) { }
    
    // DbSet das Models para que sejam reconhecidas no contexto
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }

    // Método da classe mãe que executa algo na criação das Models listadas acima nos DbSet, ideal para configurar certos comportamentos como os listados abaixo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Para cada propriedade do tipo string setta por padrão o tipo da coluna como varchar(100)
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        // Registra os Mappings
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppFornecedoresDbContext).Assembly);

        // Impedir que ao deletar uma classe, os filhos sejam levados juntos (Cascade Delete)
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        base.OnModelCreating(modelBuilder);
    }
}

