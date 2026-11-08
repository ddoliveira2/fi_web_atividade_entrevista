using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Utils
{
    public static class Utils
    {
        /// <summary>
        /// Metodo criado para padronizar a forma como manipulamos o cpf
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static string SoNumeroCPF(this string cpf)
        {
            return Regex.Replace(cpf, @"\D", "");
        }
    }
}
