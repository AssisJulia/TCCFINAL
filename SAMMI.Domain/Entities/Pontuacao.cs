using SAMMI.Domain.Entities.Base;
using SAMMI.Domain.Enumerators;

namespace SAMMI.Domain.Entities
{
    public class Pontuacao : EntityBase
    {
        public Guid UsuarioId { get; private set; }
        public Nivel Nivel { get; private set; }
        public TipoJogo TipoJogo { get; private set; }
        public DateTime Data { get; private set; }
        public int QuantidadePontos { get; private set; }

        public virtual Usuario Usuario { get; private set; }

        protected Pontuacao() { }

        public Pontuacao(
            Guid usuarioId, 
            Nivel disciplina, 
            TipoJogo tipoJogo, 
            int quantidadePontos)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            Nivel = disciplina;
            TipoJogo = tipoJogo;
            Data = DateTime.Now;
            QuantidadePontos = quantidadePontos;
        }
    }
}
