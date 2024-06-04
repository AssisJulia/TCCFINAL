using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAMMI.Domain.Entities;

namespace SAMMI.Data.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.EmailLogin)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Senha)
            .HasMaxLength(2000)
            .IsRequired();

        builder.ToTable("TB_Usuario");
    }
}