using FINANCEAPP.Core.Data;
using FINANCEAPP.Core.Helpers;
using FINANCEAPP.Core.Models;
using System.Globalization;
using System.Text; // Importado para usar StringBuilder
using System.Text.RegularExpressions;

namespace FINANCEAPP.Core.Services
{
    public class InterpretadorDeTransacaoService
    {
        // ... (As Regex não mudam)
        private static readonly Regex _regexLinhaChat = new Regex(@"^(\d{2}\/\d{2}\/\d{4})\s\d{2}:\d{2}\s-\s.+?:\s(.+)$", RegexOptions.Compiled);
        private static readonly Regex _regexValor = new Regex(@"(\d+[\.,]\d{2})|(\d+)", RegexOptions.Compiled);
        private static readonly Regex _regexParcelas = new Regex(@"(\d+)x", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

            // --- PONTO DE MELHORIA: Limpeza da Mensagem ---
            // Remove o texto "<Mensagem editada>" e outros ruídos.
            mensagem = mensagem.Replace("<Mensagem editada>", "").Trim();

            var transacao = new Transacao();

            // 1. Extrair Data e Valor
            if (DateTime.TryParse(dataString, out var data)) transacao.Data = data;
            if (decimal.TryParse(matchValor.Value.Replace(",", "."), CultureInfo.InvariantCulture, out var valor)) transacao.Valor = valor;

            var palavras = mensagem.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var descricaoBuilder = new StringBuilder();
            string? categoriaAbsoluta = null;

            // 2. Processar cada palavra da mensagem
            foreach (var palavra in palavras)
            {
                if (palavra == matchValor.Value) continue;

                var palavraNormalizada = TextoHelper.Normalizar(palavra);

                // É uma palavra-chave de sistema (Tipo, Formato, Parcela)?
                if (IsPalavraChaveSistema(palavraNormalizada, transacao))
                {
                    continue; // Se for, processe e pule para a próxima palavra.
                }

                // É uma categoria absoluta (uma chave do dicionário)?
                if (Dicionarios.Categorias.ContainsKey(palavraNormalizada))
                {
                    categoriaAbsoluta = Dicionarios.Categorias.Keys.First(k => k.Equals(palavraNormalizada, StringComparison.OrdinalIgnoreCase));
                    continue; // Categorias absolutas NUNCA entram na descrição.
                }

                // É uma sub-categoria (um valor no dicionário)?
                var categoriaEncontrada = BuscarSubCategoria(palavraNormalizada);
                if (categoriaEncontrada != null)
                {
                    // Define a categoria (pode ser sobrescrita pela absoluta depois)
                    transacao.Categoria = categoriaEncontrada;
                }
                
                // Se a palavra não for de sistema nem uma categoria absoluta, ela faz parte da descrição.
                descricaoBuilder.Append(palavra + " ");
            }

            // 3. Define a categoria final (a absoluta tem prioridade)
            if (categoriaAbsoluta != null)
            {
                transacao.Categoria = categoriaAbsoluta;
            }
            
            // 4. Define a descrição final
            transacao.Descricao = descricaoBuilder.ToString().Trim();

            // 5. Garante que a descrição não fique vazia
            if (string.IsNullOrWhiteSpace(transacao.Descricao))
            {
                transacao.Descricao = transacao.Categoria;
            }

            return ResultadoOperacao<Transacao>.CreateSuccess(transacao);
        }

        // Renomeado para maior clareza, busca apenas nos VALORES
        private string? BuscarSubCategoria(string palavraNormalizada)
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

        private bool IsPalavraChaveSistema(string palavraNormalizada, Transacao transacao)
        {
            if (Dicionarios.PalavrasEntrada.Contains(palavraNormalizada))
            {
                transacao.Tipo = TipoTransacao.Entrada;
                return true;
            }

            if (Dicionarios.FormatosPagamento.Contains(palavraNormalizada))
            {
                transacao.FormatoPagamento = palavraNormalizada;
                return true;
            }

            var matchParcela = _regexParcelas.Match(palavraNormalizada);
            if (matchParcela.Success && int.TryParse(matchParcela.Groups[1].Value, out int parcelas))
            {
                transacao.Parcelas = parcelas;
                transacao.FormatoPagamento = "credito";
                return true;
            }
            
            return false;
        }
    }
}