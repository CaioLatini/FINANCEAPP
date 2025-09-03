using System.Globalization;
using System.Text;

namespace FINANCEAPP.Core.Helpers
{
    public static class TextoHelper
    {
        public static string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            // Decompõe os caracteres acentuados (ex: 'á' -> 'a' + '´')
            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (char c in textoNormalizado)
            {
                // Remove os acentos, mantendo a letra base
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().ToLowerInvariant();
        }
    }
}