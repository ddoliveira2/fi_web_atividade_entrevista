using FI.AtividadeEntrevista.DAL.Beneficios;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoClienteBeneficios
    {
        private DaoBeneficios _cliBeneficios;

        public BoClienteBeneficios()
        {
            this._cliBeneficios = new DaoBeneficios();
        }

        /// <summary>
        /// Inclui um novo beneficio a um cliente
        /// </summary>
        /// <param name="clienteBeneficios"></param>
        /// <returns></returns>
        public long Incluir(ClienteBeneficios clienteBeneficios)
        {
            /*
               Antes de realizar o cadastro, é efetuada a sanitização do campo CPF, 
               removendo todos os caracteres não numéricos, garantindo a padronização 
               dos dados antes de sua persistência no banco de dados.
            */
            clienteBeneficios.CPF = clienteBeneficios.CPF.SoNumeroCPF();
            return this._cliBeneficios.Incluir(clienteBeneficios);
        }

        /// <summary>
        /// Altera um beneficio vinculado a um cliente
        /// </summary>
        /// <param name="clienteBeneficios"></param>
        public void Alterar(ClienteBeneficios clienteBeneficios)
        {
            /*
               Antes de realizar o cadastro, é efetuada a sanitização do campo CPF, 
               removendo todos os caracteres não numéricos, garantindo a padronização 
               dos dados antes de sua persistência no banco de dados.
            */
            clienteBeneficios.CPF = clienteBeneficios.CPF.SoNumeroCPF();
            this._cliBeneficios.Alterar(clienteBeneficios);
        }

        /// <summary>
        /// Exclui um beneficio vinculado a um cliente
        /// </summary>
        /// <param name="id"></param>
        public void Excluir(long id)
        {
            this._cliBeneficios.Excluir(id);
        }

        /// <summary>
        /// Verifica se cliente já tem um beneficio cadastrado
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerificarExistenciaBeneficios(string CPF, Int64 id, Int64 idBeneficio)
        {
            return this._cliBeneficios.VerificarExistenciaBeneficios(CPF.SoNumeroCPF(), id,idBeneficio);
        }

        /// <summary>
        /// Lista beneficios vinculados a um cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public List<ClienteBeneficios> Listar(long idCliente)
        {
            return this._cliBeneficios.Listar(idCliente);
        }        
    }
}
