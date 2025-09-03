namespace FINANCEAPP.Core.Models
{
    /// <summary>
    /// Define o tipo de transação (Entrada ou Saída).
    /// </summary>
    public enum TipoTransacao
    {
        Saida,
        Entrada
    }

    /// <summary>
    /// Representa uma única transação financeira.
    /// </summary>
    public class Transacao
    {
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = "Não definida";
        public TipoTransacao Tipo { get; set; } = TipoTransacao.Saida;
        public string FormatoPagamento { get; set; } = "Débito";
        public int Parcelas { get; set; } = 0;
    }
}