namespace FINANCEAPP.Core.Models
{
    /// <summary>
    /// Encapsula o resultado de uma operação, indicando sucesso ou falha.
    /// </summary>
    /// <typeparam name="T">O tipo do dado retornado em caso de sucesso.</typeparam>
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; private set; }
        public string? Mensagem { get; private set; }
        public T? Dados { get; private set; }

        // Construtor privado para forçar o uso dos métodos de fábrica (CreateSuccess, CreateFailure)
        private ResultadoOperacao(bool sucesso, string? mensagem, T? dados)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
        }

        /// <summary>
        /// Cria uma instância de resultado de sucesso.
        /// </summary>
        public static ResultadoOperacao<T> CreateSuccess(T dados, string? mensagem = null)
        {
            return new ResultadoOperacao<T>(true, mensagem, dados);
        }

        /// <summary>
        /// Cria uma instância de resultado de falha.
        /// </summary>
        public static ResultadoOperacao<T> CreateFailure(string mensagem)
        {
            return new ResultadoOperacao<T>(false, mensagem, default);
        }
    }
}