using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

// Mapping onde configuramos as especifidades 
public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        // Define qual campo será a primary key.
        builder.HasKey(p => p.Id);

        // Todos os campos seguem um modelo parecido, basicamente informando se é requerido e o tipo da coluna.
        builder.Property(c => c.Logradouro)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Numero)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(c => c.Cep)
            .IsRequired()
            .HasColumnType("varchar(8)");

        builder.Property(c => c.Complemento)
            .HasColumnType("varchar(250)");

        builder.Property(c => c.Bairro)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Cidade)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Estado)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.ToTable("Enderecos");
    }
}