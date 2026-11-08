using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.WebAtividadeEntrevista.Models
{
    
    public class ClienteBeneficiosModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long IdBeneficio { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public long IdCliente { get; set; }



        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }



        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [RegularExpression(@"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$", ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }
    }
}
