using SAMMI.Domain.Enumerators;

namespace SAMMI.Domain.DTOs.Pontuacao.Request
{
    public class CriarPontuacaoRequest
    {
        public Nivel Nivel { get; set; }
        public TipoJogo TipoJogo { get; set; }
        public int QuantidadePontuacao { get; set; }
    }
}
