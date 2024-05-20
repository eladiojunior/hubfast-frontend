let editorJsonRequest = null;
let editorJsonResponse = null;
EditarOperacoes = {
    
    InitConfiguracao: function () {
        EditarOperacoes.InitEditorJson();
        EditarOperacoes.InitGravarOperacao();
        $(".nova-operacao").click(function () {
            EditarOperacoes.NovaOperacao();
        });
    },
    
    InitEditorJson: function () {
        
        const options = {
            mode: 'code',
        };

        const editorRequest = document.getElementById("jsoneditor_request");
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
        EditarOperacoes.CarregarEditorRequestJson(json_request);

        const editorResponse = document.getElementById("jsoneditor_response");
        editorJsonResponse = new JSONEditor(editorResponse, options);
        let json_response = $("input[name='JsonResponseOperacao']").val();
        if (json_response === '')
            json_response = {
                "data": {},
                "mensagens": []
            };
        EditarOperacoes.CarregarEditorResponseJson(json_response);
        
    },
    
    CarregarEditorRequestJson: function (json) {
        if (!editorJsonRequest)
            return;
        editorJsonRequest.set(json);
    },
    
    CarregarEditorResponseJson: function (json) {
        if (!editorJsonResponse)
            return;
        editorJsonResponse.set(json);
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
            console.log(jsonRequest);
            const jsonResponse = editorJsonResponse.get();
            console.log(jsonResponse);
            $("input[name='JsonRequestOperacao']").val(JSON.stringify(jsonRequest));
            $("input[name='JsonResponseOperacao']").val(JSON.stringify(jsonResponse));
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
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Global.ExibirMensagem(errorThrown, true);
                }
            });
            return false;
        });
    },
    NovaOperacao: function () {
        console.log("Nova operacao.");
    },
}