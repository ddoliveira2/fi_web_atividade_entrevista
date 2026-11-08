var idBeneficio = 0;

$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #CPF').val(obj.CPF);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
    }

    $('#CPF').mask('000.000.000-00');
    $('#CEP').mask('00000-000');
    $('#Telefone').mask('(00)0000-0000');
    $('#CPF_Beneficiario').mask('000.000.000-00');

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
  
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "CPF": $(this).find("#CPF").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                //$("#formCadastro")[0].reset();                                
                //window.location.href = urlRetorno;
            }
        });
    })
    
})

$('#formCadastroBeneficiarios').submit(function (e) {
    e.preventDefault();
    var baseUrlRequest = "";
    var dataModel = {};
    if (idBeneficio != 0) {
        baseUrlRequest = urlAlterarBeneficiarios
        dataModel = {
            "IDBENEFICIO": idBeneficio,
            "IDCLIENTE": obj.Id,
            "NOME": $(this).find("#Nome_Beneficiario").val(),
            "CPF": $(this).find("#CPF_Beneficiario").val()
        };
    }
    else {
        baseUrlRequest = urlPostBeneficiarios
        dataModel = {
            "IDCLIENTE": obj.Id,
            "NOME": $(this).find("#Nome_Beneficiario").val(),
            "CPF": $(this).find("#CPF_Beneficiario").val()
        };
    }

    $.ajax({
        url: baseUrlRequest,
        method: "POST",
        data: dataModel,
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                if (r.status == 200) {
                    idBeneficio = 0;
                    atualizarTable();
                    $('.modal-title').text("Cadastrar Benefici&aacute;rio");
                    $("#formCadastroBeneficiarios")[0].reset();
                    ModalDialog("Sucesso", r.message)
                }
                else {
                    ModalDialog("Aviso!", r.message)
                }
                
                //ModalDialog("Sucesso!", r.message)
                //$("#formCadastroBeneficiarios")[0].reset();
                //window.location.href = urlRetorno;
            }
    });
})
function modalAlterarIncluirBeneficio() {

    $('#modalAlterarIncluirBeneficio').modal('show');
}

$('#tbListBeneficios').ready(function (e) {
    atualizarTable();
})
function validarCpf(controle) {

    if (!controle.value.trim()) {
        return;
    }

    var cpf = controle.value.replace(/\D/g, '');

    if (cpf.length !== 11 || /^(\d)\1{10}$/.test(cpf)) {
        ModalDialog("Aviso", "CPF informado é inválido", "0");
        controle.value = '';
        return false;
    }

    var soma = 0;
    var resto;

    // Validação do primeiro dígito
    for (var i = 1; i <= 9; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    }

    resto = (soma * 10) % 11;

    if (resto === 10 || resto === 11) {
        resto = 0;
    }

    if (resto !== parseInt(cpf.substring(9, 10))) {
        ModalDialog("Aviso", "CPF informado é inválido", "0");
        controle.value = '';
        return false;
    }

    soma = 0;

    // Validação do segundo dígito
    for (var i = 1; i <= 10; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    }

    resto = (soma * 10) % 11;

    if (resto === 10 || resto === 11) {
        resto = 0;
    }

    if (resto !== parseInt(cpf.substring(10, 11))) {
        ModalDialog("Aviso", "CPF informado é inválido", "0");
        controle.value = '';
        return false;
    }

    return true;
}
function atualizarTable(){
    $.ajax({
        url: urlPostListBeneficiarios,
        method: "POST",
        data: {
            "IDCLIENTE": obj.Id
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                carregarBeneficiarios(r.Records);
            }
    });
}

function alterarBeneficiario(id, nome, cpf) {
    $('.modal-title').text("Alterar Benefici&aacute;rio");
    idBeneficio = id;
    $('#Nome_Beneficiario').val(nome);
    $('#CPF_Beneficiario').val(cpf);
    
}

function removerBeneficiario(idBeneficio) {
    $.ajax({
        url: urlDeleteBeneficiarios,
        type: "POST",
        data: {
            "idBeneficio": idBeneficio
        },
        success: function (response) {
            atualizarTable();
            ModalDialog("Sucesso", response.message)
        },
        error: function (xhr) {

            if (xhr.status === 400) {
                ModalDialog("Ocorreu um erro", xhr.responseJSON);
            }
            else if (xhr.status === 500) {
                ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            }
            else {
                ModalDialog("Ocorreu um erro", "Não foi possível excluir os beneficiários.");
            }
        }
    });
}

function carregarBeneficiarios(lista) {

    // Limpa a tabela antes de preencher
    $('#listaBeneficiarios').empty();

    // Percorre a lista de beneficiários
    $.each(lista, function (index, beneficiario) {

        let linha = `
            <tr>
                <td>${beneficiario.CPF}</td>
                <td>${beneficiario.Nome}</td>
                <td>
                    <button type="button" class="btn btn-primary btn-sm"
                        onclick="alterarBeneficiario(${beneficiario.IdBeneficio},'${beneficiario.Nome}','${beneficiario.CPF}')">
                        Alterar
                    </button>
                    <button type="button" class="btn btn-primary btn-sm"
                        onclick="removerBeneficiario(${beneficiario.IdBeneficio})">
                        Excluir
                    </button>
                </td>
            </tr>
        `;

        $('#listaBeneficiarios').append(linha);

    });
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');    
}
