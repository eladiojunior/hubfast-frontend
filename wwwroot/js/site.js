Global = {
    ExibirMensagem: function (msg, hasErro) {
        if (!msg || msg === "") return;
        console.log(msg);
        /*
        var tituloMsg = (hasErro?"Erro":"Informação");
        var classTitulo = (hasErro?"warning":"success");
        var classIconMsg = (hasErro?"fs-5 icon ion-close-circled text-danger":"fs-5 icon ion-information-circled")
        $("body").append(`
            <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 11">
                <div id="mensagemToast" class="toast hide" role="alert" aria-live="assertive" aria-atomic="true">
                    <div class="toast-header ${classTitulo}">
                        <i class="${classIconMsg}"></i>
                        <strong class="me-auto">${tituloMsg}</strong>
                        <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Fechar"></button>
                    </div>
                    <div class="toast-body">${msg}</div>
                </div>
            </div>
        `);
        */
    },
};