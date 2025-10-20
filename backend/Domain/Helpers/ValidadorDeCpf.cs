namespace backend.Domain.Helpers
{
    public class ValidadorDeCpf
    {
        public static bool Validar(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Verifica se contém apenas dígitos
            if (!cpf.All(char.IsDigit))
                return false;

            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            // Verifica primeiro dígito
            if ((cpf[9] - '0') != primeiroDigito)
                return false;

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            // Verifica segundo dígito
            return (cpf[10] - '0') == segundoDigito;
        }
    }
}
