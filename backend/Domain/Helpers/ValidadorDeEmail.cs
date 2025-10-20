using System.Net.Mail;

namespace backend.Domain.Helpers
{
    public class ValidadorDeEmail
    {
        public static bool Validar(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
