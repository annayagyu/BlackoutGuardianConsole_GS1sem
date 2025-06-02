using System.Text.RegularExpressions;

namespace BlackoutGuardianConsole.Utils
{
    public static class ValidacaoUtils
    {
        public static bool ValidarLocalizacao(string input, out string? erro)
        {
            erro = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                erro = "Localização não pode ser vazia.";
                return false;
            }

            if (!Regex.IsMatch(input, @"^[\p{L}\s\-']+$"))
            {
                erro = "Localização deve conter apenas letras, espaços, hífens e apóstrofos.";
                return false;
            }

            return true;
        }

        public static bool ValidarDataHora(string input, out DateTime data, out string? erro)
        {
            data = default;
            erro = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                erro = "Data não pode ser vazia.";
                return false;
            }

            if (!Regex.IsMatch(input, @"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}$"))
            {
                erro = "Formato inválido. Use dd/MM/yyyy HH:mm:ss";
                return false;
            }

            if (!DateTime.TryParseExact(
                input,
                "dd/MM/yyyy HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out data))
            {
                erro = "Data inválida. Verifique os valores.";
                return false;
            }

            return true;
        }

        public static bool ValidarSeveridade(string input, out string? erro)
        {
            erro = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                erro = "Severidade não pode ser vazia.";
                return false;
            }

            if (!input.Equals("Leve", StringComparison.OrdinalIgnoreCase) &&
                !input.Equals("Moderada", StringComparison.OrdinalIgnoreCase) &&
                !input.Equals("Informativo", StringComparison.OrdinalIgnoreCase))
            {
                erro = "Severidade inválida. Use: Leve, Moderada ou Informativo";
                return false;
            }

            return true;
        }
    }
}