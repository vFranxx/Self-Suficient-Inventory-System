using System.Text.RegularExpressions;

namespace WebClient.Shared
{
    public static class Validations
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield break;
            }
            if (pw.Length < 8)
                yield return "Las contraseñas deben tener minimo 8 caracteres de largo";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Las contraseñas deben tener al menos una letra mayúscula";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Las contraseñas deben tener al menos una letra minúscula";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Las contraseñas deben tener al menos un número";
        }

        public static IEnumerable<string> PasswordMatch(string originalPassword, string confirmedPassword)
        {
            if (originalPassword != confirmedPassword)
                yield return "Las contraseñas no coinciden...";
        }
    }
}
