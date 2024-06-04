using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAMMI.Domain.Entities;

namespace SAMMI.Data.Configuration
{
    public class PontuacaoConfiguration : IEntityTypeConfiguration<Pontuacao>
    {
        public void Configure(EntityTypeBuilder<Pontuacao> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nivel)
                .IsRequired();

            builder.Property(p => p.TipoJogo)
                .IsRequired();

            builder.Property(p => p.Data)
                .IsRequired();

            builder.Property(p => p.QuantidadePontos)
                .IsRequired();

            builder.ToTable("Pontuacao");
        }
    }
}