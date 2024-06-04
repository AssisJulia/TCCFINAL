using SAMMI.Domain.Enumerators;

namespace SAMMI.Domain.DTOs.Jogo.Response
{
    public class JogoResponse
    {
        public Guid JogoId { get; set; }
        public DateTime Data { get; set; }        
        public int Pontuacao { get; set; }
        public Disciplina Disciplina { get; set; }
        public TipoJogo TipoJogo { get; set; }
    }
}
