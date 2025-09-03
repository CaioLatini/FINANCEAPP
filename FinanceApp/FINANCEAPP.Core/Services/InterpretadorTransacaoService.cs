using FINANCEAPP.Core.Data;
using FINANCEAPP.Core.Helpers;
using FINANCEAPP.Core.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FINANCEAPP.Core.Services
{
    /// <summary>
    /// Serviço responsável por receber uma string e transcrevela em transações
    /// </summary>
    public class InterpretadorDeTransacaoService
    {
        // Expressões Regulares (Regex) para encontrar padrões no texto
        private static readonly Regex _regexLinhaChat = new Regex(@"^(\d{2}\/\d{2}\/\d{4})\s\d{2}:\d{2}\s-\s.+?:\s(.+)$", RegexOptions.Compiled);
        private static readonly Regex _regexValor = new Regex(@"(\d+[\.,]\d{2})|(\d+)", RegexOptions.Compiled);
        private static readonly Regex _regexParcelas = new Regex(@"(\d+)x", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Lê todas as palavras de uma string e trata o texto.
        /// </summary>
        /// <param name="linhaDoChat">String contendo as informações de uma transação.txt.</param>
        /// <returns>Um ResultadoOperacao contendo uma transação tratada</returns>        
        public ResultadoOperacao<Transacao> Interpretar(string linhaDoChat)
        {
            var matchLinha = _regexLinhaChat.Match(linhaDoChat);
            if (!matchLinha.Success)
            {
                return ResultadoOperacao<Transacao>.CreateFailure("Linha não é uma mensagem de chat válida.");
            }

            var dataString = matchLinha.Groups[1].Value;
            var mensagem = matchLinha.Groups[2].Value;

            var matchValor = _regexValor.Match(mensagem);
            if (!matchValor.Success)
            {
                return ResultadoOperacao<Transacao>.CreateFailure("Mensagem não contém um valor monetário.");
            }

            // --- Se a linha é válida, começamos a montar a transação ---
            var transacao = new Transacao();

            // 1. Extrair Data e Valor
            if (DateTime.TryParse(dataString, out var data))
            {
                transacao.Data = data;
            }
            if (decimal.TryParse(matchValor.Value.Replace(",", "."), CultureInfo.InvariantCulture, out var valor))
            {
                transacao.Valor = valor;
            }

            // 2. Processar cada palavra da mensagem
            var palavras = mensagem.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var ultimaPalavraDaLista = palavras.Last();

            foreach (var palavra in palavras)
            {
                // Ignora a palavra que é o próprio valor
                if (palavra == matchValor.Value) continue;

                // Identifica se é a ultima palavra 
                bool isUltimaPalavra = (palavra == ultimaPalavraDaLista);

                var palavraNormalizada = TextoHelper.Normalizar(palavra);

                // 3. Identificar Tipo (Entrada/Saída), Formato e Parcelas
                if (Dicionarios.PalavrasEntrada.Contains(palavraNormalizada))
                {
                    transacao.Tipo = TipoTransacao.Entrada;
                    continue;
                }

                if (Dicionarios.FormatosPagamento.Contains(palavraNormalizada))
                {
                    transacao.FormatoPagamento = palavraNormalizada;
                    continue;
                }

                var matchParcela = _regexParcelas.Match(palavraNormalizada);
                if (matchParcela.Success && int.TryParse(matchParcela.Groups[1].Value, out int parcelas))
                {
                    transacao.Parcelas = parcelas;
                    transacao.FormatoPagamento = "credito";
                    continue;
                }

                // 4. Identificar Categoria
                var categoriaEncontrada = BuscarCategoria(palavraNormalizada);
                if (!string.IsNullOrEmpty(categoriaEncontrada))
                {
                    transacao.Categoria = categoriaEncontrada;
                    continue;
                }

                // 5. O que sobrar, vira descrição
                transacao.Descricao += " " + palavraNormalizada;

                // 6. Garante que a descrição não fique vazia
                if (transacao.Descricao == string.Empty && isUltimaPalavra)
                {
                    transacao.Descricao = transacao.Categoria;
                    continue;
                }
            }
            return ResultadoOperacao<Transacao>.CreateSuccess(transacao);
        }

        private string? BuscarCategoria(string palavraNormalizada)
        {
            foreach (var parCategoria in Dicionarios.Categorias)
            {
                if (parCategoria.Value.Contains(palavraNormalizada))
                {
                    return parCategoria.Key;
                }
            }
            return null;
        }
    }
}