using FINANCEAPP.Core.Models;

namespace FINANCEAPP.Core.Services
{
    /// <summary>
    /// Serviço responsável por ler o conteúdo de arquivos de texto.
    /// </summary>
    public class LeitorDeChatService
    {
        /// <summary>
        /// Lê todas as linhas de um arquivo de texto.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo .txt.</param>
        /// <returns>Um ResultadoOperacao contendo um array de strings com as linhas do arquivo.</returns>
        public ResultadoOperacao<string[]> LerLinhas(string caminhoArquivo)
        {
            // Validamos a existencia, validade e na sequencia lemos o arquivo.
            if (!File.Exists(caminhoArquivo))
            {
                return ResultadoOperacao<string[]>.CreateFailure($"O arquivo não foi encontrado no caminho: {caminhoArquivo}");
            }

            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                return ResultadoOperacao<string[]>.CreateFailure("O caminho do arquivo não pode ser vazio.");
            }

            try
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);
                return ResultadoOperacao<string[]>.CreateSuccess(linhas, "Arquivo lido com sucesso.");
            }

            catch (Exception ex)
            {
                return ResultadoOperacao<string[]>.CreateFailure($"Ocorreu um erro inesperado ao ler o arquivo: {ex.Message}");
            }
        }
    }
}