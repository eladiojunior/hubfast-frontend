AuthorizationIntegracao = {
    InitGravarAuthorization: function () {
        
        $('.tipo-autorizacao').change(function () {
            AuthorizationIntegracao.CarregarTipoAuthorization();
        });
        
        $('#form-autorizacao').submit(function (event) {
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            $(this).addClass('was-validated');
            $.ajax({
                cache: false,
                type: "POST",
                url: _contexto + "AuthorizationIntegracao/GravarAutorizacao",
                dataType: "json",
                data: $(this).serialize(),
                success: function (result) {
                    if (result.hasErro) {
                        Global.ExibirMensagem(result.erros, true);
                        return;
                    }
                    Global.ExibirMensagem(result.mensagem);
                    AuthorizationIntegracao.ObterAuthorization();
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Global.ExibirMensagem(errorThrown, true);
                }
            });
            event.preventDefault();
            event.stopPropagation();
            return false;
        });
    },
    ObterAuthorization: function () {
        const idIntegracao = $("#IdIntegracao").val();
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "AuthorizationIntegracao/ObterAutorizacao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.authorization").html(result.model);
                AuthorizationIntegracao.CarregarTipoAuthorization();
                AuthorizationIntegracao.InitGravarAuthorization();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    },
    CarregarTipoAuthorization: function () {
        const idIntegracao = $("#IdIntegracao").val();
        const tipoAuthorization = $("#CodigoTipoAutorizacao").val();
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "AuthorizationIntegracao/ObterTipoAutorizacao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao,
                codigoTipoAutenticacao: tipoAuthorization
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.tipo-autorizacao-selecionada").html(result.model);
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    }
}