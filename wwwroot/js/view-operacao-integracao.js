let editorJsonRequest = null;
let editorJsonResponse = null;
OperacaoIntegracao = {
    
    InitConfiguracao: function () {
        OperacaoIntegracao.InitEditorJson();
        OperacaoIntegracao.InitGravarOperacao();
        $(".nova-operacao").click(function () {
            OperacaoIntegracao.NovaOperacao();
        });
        OperacaoIntegracao.ListarOperacoes();
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
        $('#form-operacao').submit(async function (event) {
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            $(this).addClass('was-validated');
            //Verificar erros no JSONs
            const errorsJsonRequest = await editorJsonRequest.validate();
            if (errorsJsonRequest.length) {
                Global.ExibirMensagem(errorsJsonRequest[0].mensage, true);
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            const errorsJsonResponse = await editorJsonResponse.validate();
            if (errorsJsonResponse.length) {
                Global.ExibirMensagem(errorsJsonResponse[0].mensage, true);
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            //Carregar JSON nos inputs
            const jsonRequest = editorJsonRequest.get();
            const jsonResponse = editorJsonResponse.get();
            $("input[name='JsonRequestOperacao']").val(JSON.stringify(jsonRequest));
            $("input[name='JsonResponseOperacao']").val(JSON.stringify(jsonResponse));
            $.ajax({
                cache: false,
                type: "POST",
                url: _contexto + "OperacaoIntegracao/GravarOperacao",
                dataType: "json",
                data: $(this).serialize(),
                success: function (result) {
                    if (result.hasErro) {
                        Global.ExibirMensagem(result.erros, true);
                        return;
                    }
                    const idIntegracao = result.model.idIntegracao;
                    const idOperacao = result.model.idOperacao;
                    Global.ExibirMensagem(result.mensagem);
                    OperacaoIntegracao.CarregarOperacao(idIntegracao, idOperacao);
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Global.ExibirMensagem(errorThrown, true);
                }
            });
            event.preventDefault();
            event.stopPropagation();
            return false;
        });
    },
    NovaOperacao: function () {
        const idIntegracao = $("#IdIntegracao").val();
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "OperacaoIntegracao/NovaOperacao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.operacoes").html(result.model);
                OperacaoIntegracao.InitConfiguracao();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
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
            OperacaoIntegracao.CarregarOperacao(idIntegracao, idOperacao);
        });
        $(".remover-operacao").click(function () {
            const idOperacao = $(this).data('id');
            $("#IdOperacao").val(idOperacao);
            Global.ExibirConfirmacao("Remover Operação da Integração", "Confirma a remoção da operação ["+idOperacao+"] da integração?", 
                OperacaoIntegracao.RemoverOperacao, null, "modalConfirmaRemocaoOperacao");
        });
    },
    CarregarOperacao: function (idIntegracao, idOperacao) {
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "OperacaoIntegracao/CarregarEdicaoOperacao",
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
                OperacaoIntegracao.InitConfiguracao();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    },
    RemoverOperacao: function () {
        const idOperacao = $("#IdOperacao").val();
        $.ajax({
            cache: false,
            type: "DELETE",
            url: _contexto + "OperacaoIntegracao/RemoverOperacao",
            dataType: "json",
            data: {
                idOperacao: idOperacao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                OperacaoIntegracao.NovaOperacao();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    },
    ListarOperacoes: function () {
        const idIntegracao = $("#IdIntegracao").val();
        $.ajax({
            cache: false,
            type: "GET",
            url: _contexto + "OperacaoIntegracao/ListarOperacao",
            dataType: "json",
            data: {
                idIntegracao: idIntegracao
            },
            success: function (result) {
                if (result.hasErro) {
                    Global.ExibirMensagem(result.erros, true);
                    return;
                }
                $("div.lista-operacoes").html(result.model);
                OperacaoIntegracao.InitConfiguracaoListaOperacoes();
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                Global.ExibirMensagem(errorThrown, true);
            }
        });
    }
}