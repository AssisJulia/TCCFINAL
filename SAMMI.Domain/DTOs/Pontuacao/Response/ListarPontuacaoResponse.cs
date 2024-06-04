namespace SAMMI.Domain.DTOs.Pontuacao.Response
{
    public class ListarPontuacaoResponse
    {
        public Guid UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public DateTime Data { get; set; }
        public string Nivel { get; set; }
        public string TipoJogo { get; set; }
        public int QuantidadePontuacao { get; set; }
    }
}
