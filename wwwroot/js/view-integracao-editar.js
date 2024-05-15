EditarIntegracao = {

    InitConfiguracao: function () {
        $("input[name='nomeIntegracao']").on('keyup', function (event) {
            let nome = $(this).val();
            if (nome === '')
                nome = '{nome_integracao}';
            $("label.nome-integracao").text(nome);
        });
        $('#tabsConfiguracao a').on('click', function (event) {
            var idIntegracao = $("#IdIntegracao").val();
            var nome_tab = $(this).data('tab');
            console.log(nome_tab);
            if ('tab_operacoes' === nome_tab) {
                EditarIntegracao.CarregarOperacoes(idIntegracao);
            }
            event.preventDefault();
            $(this).tab('show');
        });
        $('#integrationForm').submit(function(event){
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            $(this).addClass('was-validated');
        });
    },
    CarregarOperacoes: function (idIntegracao) {
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "Integracao/ListarOperacoesIntegracao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao
            },
            success: function (result) {
                if (result.hasErro) {
                    console.log(result.erros);
                    return;
                }
                $("div.operacoes").html(result.model);
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    },
}
$(function () {
    EditarIntegracao.InitConfiguracao();
});