using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        private DaoCliente cli;
        public BoCliente()
        {
            cli = new DaoCliente();
        }
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            /*
                Antes de realizar o cadastro, é efetuada a sanitização do campo CPF, 
                removendo todos os caracteres não numéricos, garantindo a padronização 
                dos dados antes de sua persistência no banco de dados.
             */
            cliente.CPF = cliente.CPF.SoNumeroCPF();

            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            /*
                Antes de realizar a altração, é efetuada a sanitização do campo CPF, 
                removendo todos os caracteres não numéricos, garantindo a padronização 
                dos dados antes de sua persistência no banco de dados.
             */
            cliente.CPF = cliente.CPF.SoNumeroCPF();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, Int64 id)
        {
            /*
             Antes de verificar o cpf, padronizamos deixando apenas números
             */
            return cli.VerificarExistencia(CPF.SoNumeroCPF(), id);
        }


    }
}
