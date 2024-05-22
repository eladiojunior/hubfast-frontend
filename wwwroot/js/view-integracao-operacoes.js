let editorJsonRequest = null;
let editorJsonResponse = null;
EditarOperacoes = {
    
    InitConfiguracao: function () {
        EditarOperacoes.InitEditorJson();
        EditarOperacoes.InitGravarOperacao();
        $(".nova-operacao").click(function () {
            EditarOperacoes.NovaOperacao();
        });
        EditarOperacoes.InitConfiguracaoListaOperacoes();
    },
    
    InitEditorJson: function () {
        
        const options = {
            mode: 'code',
        };

        const editorRequest = document.getElementById("jsoneditor_request");
        if (editorRequest) {
            editorJsonRequest = new JSONEditor(editorRequest, options);
            let json_request = $("input[name='JsonRequestOperacao']").val();
            if (json_request === '')
                json_request = {
                    "atributo-texto": "texto",
                    "atributo-numero": 0,
                    "atributo-objeto": {
                        "atributo-texto": "texto",
                        "atributo-numero": 0
                    }
                };
            else
                json_request = JSON.parse(json_request);
            editorJsonRequest.set(json_request);
        }
        
        const editorResponse = document.getElementById("jsoneditor_response");
        if (editorResponse) {
            editorJsonResponse = new JSONEditor(editorResponse, options);
            let json_response = $("input[name='JsonResponseOperacao']").val();
            if (json_response === '')
                json_response = { "data": {}, "mensagens": [] };
            else 
                json_response = JSON.parse(json_response);
            editorJsonResponse.set(json_response);
        }
    },
    
    InitGravarOperacao: function () {
        $('#form-operacao').submit(function(event){
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            $(this).addClass('was-validated');
            //Carregar JSON nos inputs
            const jsonRequest = editorJsonRequest.get();
            const jsonResponse = editorJsonResponse.get();
            $("input[name='JsonRequestOperacao']").val(JSON.stringify(jsonRequest));
            $("input[name='JsonResponseOperacao']").val(JSON.stringify(jsonResponse));
            const idIntegracao = $("#IdIntegracao").val();
            $.ajax({
                cache: false,
                type: "POST",
                url: _contexto + "Integracao/GravarOperacao",
                dataType: "json",
                data: $(this).serialize(),
                success: function (result) {
                    if (result.hasErro) {
                        Global.ExibirMensagem(result.erros, true);
                        return;
                    }
                    Global.ExibirMensagem(result.mensagem);
                    EditarIntegracao.CarregarOperacoes(idIntegracao);
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Global.ExibirMensagem(errorThrown, true);
                }
            });
            return false;
        });
    },
    NovaOperacao: function () {
        const idIntegracao = $("#IdIntegracao").val();
        EditarIntegracao.CarregarOperacoes(idIntegracao);
    },
    InitConfiguracaoListaOperacoes: function () {
        const json_view = document.getElementById("jsoneditor_view_modal");
        if (!json_view) return;
        const viewJson = new JSONEditor(document.getElementById("jsoneditor_view_modal"), { mode: 'view' });
        $(".exibir-json").click(function () {
            let tipo = $(this).data('tipo');
            let json = $(this).data('json');
            $("#titulo_modal").html(tipo);
            viewJson.set(json);
            $("#modal_json").modal('show');
        });
        $(".editar-operacao").click(function () {
            const idOperacao = $(this).data('id');
            const idIntegracao = $("#IdIntegracao").val(); 
            EditarOperacoes.CarregarOperacao(idIntegracao, idOperacao);
        });
        $(".remover-operacao").click(function () {
            const idOperacao = $(this).data('id');
            Global.ExibirConfirmacao("Remover Operação da Integração", "Confirma a remoção da operação ["+idOperacao+"] da integração.", 
                EditarOperacoes.RemoverOperacao, null, "modalConfirmaRemocaoOperacao");
        });
    },
    CarregarOperacao: function (idIntegracao, idOperacao) {
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "Integracao/CarregarEdicaoOperacao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao,
                idOperacao: idOperacao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.operacoes").html(result.model);
                EditarOperacoes.InitConfiguracao();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    },
    RemoverOperacao: function () {
        const idOperacao = $("#IdOperacao").val();
        const idIntegracao = $("#IdIntegracao").val();
        $.ajax({
            cache: false,
            type: "POST",
            url: _contexto + "Integracao/RemoverOperacao",
            dataType: "json",
            data: {
                idOperacao: idOperacao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                EditarIntegracao.CarregarOperacoes(idIntegracao);
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    }
}