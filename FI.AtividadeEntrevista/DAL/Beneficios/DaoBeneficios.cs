using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.DAL.Beneficios
{
    internal class DaoBeneficios : AcessoDados
    {

        /// <summary>
        /// Inclui um novo beneficio
        /// </summary>
        /// <param name="clienteBeneficios"></param>
        /// <returns></returns>
        internal long Incluir(DML.ClienteBeneficios clienteBeneficios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", clienteBeneficios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", clienteBeneficios.IdCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", clienteBeneficios.CPF));

            DataSet ds = base.Consultar("FI_SP_IncClienteBeneficios", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Lista os beneficios de um cliente
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        internal List<DML.ClienteBeneficios> Listar(long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ConsClienteBeneficios", parametros);
            List<DML.ClienteBeneficios> beneficios = Converter(ds);

            return beneficios;
        }

        /// <summary>
        /// Verifica se um beneficio já foi cadastrado para um cliente
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        internal bool VerificarExistenciaBeneficios(string CPF, Int64 idCliente,Int64 idBeneficio)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", idCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("idBeneficio", idBeneficio));

            DataSet ds = base.Consultar("FI_SP_VerificaClienteBeneficios", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Altera informações de beneficio de um cliente
        /// </summary>
        /// <param name="clienteBeneficios"></param>
        internal void Alterar(DML.ClienteBeneficios clienteBeneficios)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", clienteBeneficios.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", clienteBeneficios.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDBENEFICIO", clienteBeneficios.IdBeneficio));

            base.Executar("FI_SP_AltBenef", parametros);
        }

        /// <summary>
        /// Exclui um beneficio de um cliente
        /// </summary>
        /// <param name="Id"></param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelClienteBeneficio", parametros);
        }

        /// <summary>
        /// Metodo responsavel por ler o DataSet e retornar a lista do objeto ClienteBeneficios
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<DML.ClienteBeneficios> Converter(DataSet ds)
        {
            List<DML.ClienteBeneficios> lista = new List<DML.ClienteBeneficios>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.ClienteBeneficios cliBeneficios = new DML.ClienteBeneficios();
                    cliBeneficios.IdBeneficio = row.Field<long>("Id");
                    cliBeneficios.IdCliente = row.Field<long>("IdCliente");
                    cliBeneficios.Nome = row.Field<string>("Nome");
                    cliBeneficios.CPF = Regex.Replace(row.Field<string>("CPF"), @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
                    lista.Add(cliBeneficios);
                }
            }
            return lista;
        }
    }
}
