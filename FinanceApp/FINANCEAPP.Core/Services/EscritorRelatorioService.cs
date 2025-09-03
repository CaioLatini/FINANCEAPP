using FINANCEAPP.Core.Models;
using System.Text;
using System.Globalization;

namespace FINANCEAPP.Core.Services
{
    /// <summary>
    /// Serviço responsável por escrever a lista de transações em um arquivo CSV.
    /// </summary>
    public class EscritorDeRelatorioService
    {
        /// <summary>
        /// Gera um arquivo CSV a partir de uma lista de transações.
        /// </summary>
        /// <param name="transacoes">A lista de transações a ser escrita.</param>
        /// <param name="caminhoArquivo">O caminho completo onde o arquivo .csv será salvo.</param>
        /// <returns>Um ResultadoOperacao indicando sucesso ou falha.</returns>
        public ResultadoOperacao<bool> EscreverCsv(List<Transacao> transacoes, string caminhoArquivo)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                return ResultadoOperacao<bool>.CreateFailure("O caminho do arquivo não pode ser vazio.");
            }

            try
            {
                var csvBuilder = new StringBuilder();

                // 1. Adiciona o cabeçalho do CSV
                csvBuilder.AppendLine("Data;Valor;Descricao;Categoria;Tipo;FormatoPagamento;Parcelas");

                // 2. Adiciona cada transação como uma nova linha
                foreach (var transacao in transacoes)
                {
                    // Formata a data para o padrão brasileiro
                    var dataFormatada = transacao.Data.ToString("dd/MM/yyyy");
                    
                    // Converte o valor para formato brl
                    var valorFormatado = transacao.Valor.ToString("F2", new CultureInfo("pt-BR"));

                    csvBuilder.AppendLine(
                        $"{dataFormatada};" +
                        $"{valorFormatado};" +
                        $"{transacao.Descricao};" +
                        $"{transacao.Categoria};" +
                        $"{transacao.Tipo};" +
                        $"{transacao.FormatoPagamento};" +
                        $"{transacao.Parcelas}"
                    );
                }

                // 3. Escreve o conteúdo gerado no arquivo
                File.WriteAllText(caminhoArquivo, csvBuilder.ToString(), Encoding.UTF8);

                return ResultadoOperacao<bool>.CreateSuccess(true, "Relatório CSV gerado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao<bool>.CreateFailure($"Ocorreu um erro inesperado ao escrever o arquivo CSV: {ex.Message}");
            }
        }
    }
}