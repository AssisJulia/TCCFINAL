using SAMMI.Domain.Enumerators;

namespace SAMMI.Domain.DTOs.Jogo.Request
{
    public class CriarJogoRequest
    {
        public Disciplina Disciplina { get; set; }
        public TipoJogo TipoJogo { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
