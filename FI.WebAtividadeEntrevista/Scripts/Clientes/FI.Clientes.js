
$(document).ready(function () {

    $('#CPF').mask('000.000.000-00');
    $('#CEP').mask('00000-000');
    $('#Telefone').mask('(00)0000-0000');

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
                    ModalDialog("Ocorreu um erro", r.message,'0');
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.",'0');
            },
            success:
            function (r) {                                
                ModalDialog("Sucesso!", r.message, r.id);
            }
        });
    })


    
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
function redirectUpdate(id,randon) {
    if (id != '0' && id != undefined) {
        window.location.href = "/Cliente/Alterar/" + id;
    }
    else {
        $('#modalCadastroCliente' + randon).modal("hide");
    }
    
}

function ModalDialog(titulo, texto, id) {
    var random = parseFloat(Math.random().toString().replace('.', '').substring(0, 5));
    
    var texto = '<div id="modalCadastroCliente'  + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" onclick="redirectUpdate(' + id + ',' + random +  ');" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" onclick="redirectUpdate(' + id + ',' + random +  ');">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#modalCadastroCliente' + random).modal('show');
}

function modalAlterarIncluirBeneficio() {

    ModalDialog("Aviso","Para cadastrar os beneficiários, é necessário realizar previamente o cadastro do cliente.",'0')
}

function IncluirBenficiarioModalDialog() {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">Incluir Beneficiários</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>Para incluir beneficiários, é necessário realizar previamente o cadastro do cliente!</p>                                                                           ' +
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

