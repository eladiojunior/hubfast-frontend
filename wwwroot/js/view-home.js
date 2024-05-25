Home = {
    InitConfiguracao: function () {
        $('.remover-integracao').click(function () {
            const idIntegracao = $(this).data('id');
            $("#idIntegracao").val(idIntegracao);
            Global.ExibirConfirmacao("Remover Integração", "Deseja remover a integração ["+idIntegracao+"]?", 
                Home.RemoverIntegracao, null, "modalConfirmacaoRemocaoIntegracao");
        });
    },
    ListarIntegracoes: function () {
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "Integracao/ListarIntegracoes",
            dataType: "json",
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.lista-integracoes").html(result.model);
                Home.InitConfiguracao();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    },
    RemoverIntegracao: function () {
        const idIntegracao = $("#IdIntegracao").val();
        $.ajax({
            cache: false,
            type: "DELETE",
            url: _contexto + "Integracao/RemoverIntegracao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                Home.ListarIntegracoes();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    }
}
$(function () {
    Home.ListarIntegracoes();
});