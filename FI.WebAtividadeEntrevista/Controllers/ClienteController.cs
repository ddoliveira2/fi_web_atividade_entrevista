using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using FI.WebAtividadeEntrevista.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private BoClienteBeneficios boClienteBeneficio;
        private BoCliente boCliente;
        private Utils utils;


        public ClienteController() {
            this.boClienteBeneficio = new BoClienteBeneficios();
            this.boCliente = new BoCliente();
            this.utils = new Utils();
        }

        /// <summary>
        /// Construtor padão 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Construtor padão 
        /// </summary>
        /// <returns></returns>
        public ActionResult Incluir()
        {
            return View();
        }

        /// <summary>
        /// Registra um novo cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {           

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (this.utils.ValidarCpf(model.CPF))
                {
                    /*
                     Foi necessário ajustar o método VerificarExistencia, pois a implementação anterior não considerava o cenário de edição de registros. 
                     Dessa forma, ao alterar um cliente, a consulta identificava o próprio CPF já cadastrado como duplicidade. 
                     A correção passou a desconsiderar o registro atual durante a verificação, permitindo identificar apenas CPFs vinculados a outros registros.
                     */
                    if (!this.boCliente.VerificarExistencia(model.CPF, model.Id))
                    {
                        model.Id = this.boCliente.Incluir(new Cliente()
                        {
                            CEP = model.CEP,
                            Cidade = model.Cidade,
                            Email = model.Email,
                            Estado = model.Estado,
                            Logradouro = model.Logradouro,
                            Nacionalidade = model.Nacionalidade,
                            Nome = model.Nome,
                            Sobrenome = model.Sobrenome,
                            CPF = model.CPF,
                            Telefone = model.Telefone
                        });


                        return Json(new { status=200, message= "Cadastro efetuado com sucesso", id = model.Id });                        
                    }
                    else
                    {
                        return Json(new { status = 400, message = string.Format("Já existe um cliente cadastrado com o CPF {0}!", model.CPF), id = 0 });                        
                    }
                }
                else
                {
                    return Json(new { status = 400, message = string.Format("O CPF informado {0} é inválido!", model.CPF), id = 0 });                    
                }
            }
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {          
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (this.utils.ValidarCpf(model.CPF))
                {
                    /*
                     Foi necessário ajustar o método VerificarExistencia, pois a implementação anterior não considerava o cenário de edição de registros. 
                     Dessa forma, ao alterar um cliente, a consulta identificava o próprio CPF já cadastrado como duplicidade. 
                     A correção passou a desconsiderar o registro atual durante a verificação, permitindo identificar apenas CPFs vinculados a outros registros.
                     */
                    if (!this.boCliente.VerificarExistencia(model.CPF, model.Id))
                    {
                        this.boCliente.Alterar(new Cliente()
                        {
                            Id = model.Id,
                            CEP = model.CEP,
                            Cidade = model.Cidade,
                            Email = model.Email,
                            Estado = model.Estado,
                            Logradouro = model.Logradouro,
                            Nacionalidade = model.Nacionalidade,
                            Nome = model.Nome,
                            Sobrenome = model.Sobrenome,
                            CPF = model.CPF,
                            Telefone = model.Telefone
                        });

                        return Json("Cadastro alterado com sucesso");
                    }
                    else
                    {
                        return Json(string.Format("Já existe um cliente cadastrado com o CPF {0}!", model.CPF));
                    }
                }
                else
                {
                    return Json(string.Format("O CPF informado {0} é inválido!", model.CPF));
                }
            }
        }

        /// <summary>
        /// Carrega informações de um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Alterar(long id)
        {    
            Cliente cliente = this.boCliente.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone
                };


            }

            return View(model);
        }

        /// <summary>
        /// Carrega a lista de clientes
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = this.boCliente.Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Inclui benefícios para um cliente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IncluirBeneficiario(ClienteBeneficiosModel model)
        {

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                return Json(new
                {
                    message = string.Join(Environment.NewLine, erros),
                    status = 400
                });
            }
            else
            {

                if (this.utils.ValidarCpf(model.CPF))
                {
                    if (!boClienteBeneficio.VerificarExistenciaBeneficios(model.CPF, model.IdCliente, model.IdBeneficio))
                    {
                        boClienteBeneficio.Incluir(new ClienteBeneficios()
                        {
                            CPF = model.CPF,
                            Nome = model.Nome,
                            IdCliente = model.IdCliente
                        });

                        return Json(new
                        {
                            message = "Benefício cadastrado com sucesso!",
                            status = 200
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = string.Format("O CPF informado {0} já esta cadastrado para esse cliente!", model.CPF),
                            status = 400
                        });
                    }

                }
                else
                {
                    return Json(new
                    {
                        message = string.Format("O CPF informado {0} é inválido!", model.CPF),
                        status = 400
                    });
                }
            }
        }

        /// <summary>
        /// Lista os benefício de um cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClienteBeneficiosList(long idCliente)
        {
            try
            {              
                List<ClienteBeneficios> clientesBeneficios = new List<ClienteBeneficios>();

                clientesBeneficios = this.boClienteBeneficio.Listar(idCliente);

                return Json(new { Result = "OK", Records = clientesBeneficios });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Exclui itens de benefício cadastrados para os clientes
        /// </summary>
        /// <param name="idBeneficio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcluirBeneficiario(long idBeneficio)
        {
            try
            {

                this.boClienteBeneficio.Excluir(idBeneficio);

                return Json(new { status = 200, message="Benefício excluido com sucesso!"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Altera itens de benefício cadastrados para os clientes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AlterarBeneficiario(ClienteBeneficiosModel model)
        { 
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                return Json(new
                {
                    message = string.Join(Environment.NewLine, erros),
                    status = 400
                });
            }
            else
            {

                if (this.utils.ValidarCpf(model.CPF))
                {
                    if (!this.boClienteBeneficio.VerificarExistenciaBeneficios(model.CPF, model.IdCliente, model.IdBeneficio))
                    {
                        this.boClienteBeneficio.Alterar(new ClienteBeneficios()
                        {
                            IdBeneficio= model.IdBeneficio,
                            CPF = model.CPF,
                            Nome = model.Nome,
                            IdCliente = model.IdCliente
                        });

                        return Json(new
                        {
                            message = "Benefício alterado com sucesso!",
                            status = 200
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = string.Format("O CPF informado {0} já esta cadastrado para esse cliente!", model.CPF),
                            status = 400
                        });
                    }

                }
                else
                {
                    return Json(new
                    {
                        message = string.Format("O CPF informado {0} é inválido!", model.CPF),
                        status = 400
                    });
                }
            }
        }
    }
}