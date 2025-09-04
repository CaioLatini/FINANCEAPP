using System.Globalization;
using System.Text;
using FINANCEAPP.Core.Models;

namespace FINANCEAPP.Core.Helpers
{
    /// <summary>
    /// Validações de diretorios e arquivos 
    /// </summary>
    public static class ValidacaoArquivoHelper
    {
        /// <summary>
        /// Lê todas as linhas de um arquivo de texto.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo .txt.</param>
        /// <returns>ResultadoOperacao confirmando o diretorio</returns>
        public static ResultadoOperacao<bool> ValidarDiretorio(string caminhoArquivo)
        {
            if (!Directory.Exists(caminhoArquivo))
            {
                return ResultadoOperacao<bool>.CreateFailure("Diretorio não encontrado");
            }
            return ResultadoOperacao<bool>.CreateSuccess(true, "Diretorio válido");
        }
        
        /// <summary>
        /// Lê todas as linhas de um arquivo de texto.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo .txt.</param>
        /// <returns>ResultadoOperacao confirmando a validade do arquivo</returns>
        public static ResultadoOperacao<bool> ValidarArquivo(string caminhoArquivo)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                return ResultadoOperacao<bool>.CreateFailure("Arquivo inválido, não pode ser vazio");
            }
            return ResultadoOperacao<bool>.CreateSuccess(true, "Arquivo válido");
        }
    }
}