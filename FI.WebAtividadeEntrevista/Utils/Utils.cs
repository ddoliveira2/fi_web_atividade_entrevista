using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.WebAtividadeEntrevista.Utils
{
    public class Utils
    {
        /// <summary>
        /// ValidarCpf
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool ValidarCpf(string cpf)
        {


            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"\D", "");

            // CPF deve possuir exatamente 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Rejeita CPFs com todos os dígitos iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            int soma = 0;

            // Calcula o primeiro dígito verificador
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            if (primeiroDigito != cpf[9] - '0')
                return false;

            soma = 0;

            // Calcula o segundo dígito verificador
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            if (segundoDigito != cpf[10] - '0')
                return false;

            return true;
        }
    }
}
