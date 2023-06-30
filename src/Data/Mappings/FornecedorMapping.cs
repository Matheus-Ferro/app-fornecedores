using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;
public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(f => f.Documento)
            .IsRequired()
            .HasColumnType("varchar(14)");

        // As classes que possuem filhas são quem configuram as relações
        // 1 : 1 => Fornecedor : Endereco
        builder.HasOne(f => f.Endereco)
            .WithOne(e => e.Fornecedor);

        // 1 : N => Fornecedor : Endereco
        builder.HasMany(f => f.Produtos)
            .WithOne(p => p.Fornecedor)
            .HasForeignKey( p => p.FornecedorId);

        builder.ToTable("Fornecedores");
    }
}
