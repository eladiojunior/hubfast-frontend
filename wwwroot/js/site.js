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
    ExibirConfirmacao: function (titulo, mensagem, callbackSim, callbackNao, idModal) {
        if (idModal == null || idModal.trim() == '') { idModal = "ModalConfirmacao"; }
        var objModelConfirmacao = $("#" + idModal).attr("id");
        if (objModelConfirmacao) 
        {//Existe objeto Model de confirmação... reutilizar.
            $("#titulo_" + idModal).val(titulo);
            $("#mensagem_" + idModal).val(mensagem);
        }
        else
        {//Criar um objeto novo, no BODY!
            var htmlModel = "";
            if (titulo == null || titulo.trim() == '') { titulo = "Confirmação"; }
            if (mensagem == null || mensagem.trim() == '') { mensagem = "Por favor, confirme a operação?"; }
            htmlModel += "<div class=\"modal fade\" id=\"" + idModal + "\" data-bs-backdrop=\"static\" data-bs-keyboard=\"false\" tabindex=\"-1\" aria-labelledby=\"titulo_" + idModal + "\" aria-hidden=\"true\">";
            htmlModel += "	<div class=\"modal-dialog modal-dialog-centered\">";
            htmlModel += "		<div class=\"modal-content\">";
            htmlModel += "			<div class=\"modal-header\">";
            htmlModel += "				<h5 class=\"modal-title\" id=\"titulo_" + idModal + "\">" + titulo + "</h5>";
            htmlModel += "				<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" aria-label=\"Fechar\"></button>";
            htmlModel += "			</div>";
            htmlModel += "			<div class=\"modal-body\">";
            htmlModel += "			<p id=\"mensagem_" + idModal + "\">" + mensagem + "</p>";
            htmlModel += "			</div>";
            htmlModel += "			<div class=\"modal-footer\">";
            htmlModel += "			<button type=\"button\" class=\"btn btn-primary\" id=\"btnSim_" + idModal + "\" data-bs-dismiss=\"modal\">Sim</button>";
            htmlModel += "			<button type=\"button\" class=\"btn btn-secondary\" id=\"btnNao_" + idModal + "\" data-bs-dismiss=\"modal\">Não</button>";
            htmlModel += "			</div>";
            htmlModel += "		</div>";
            htmlModel += "	</div>";
            htmlModel += "</div>";
            $("body").append(htmlModel);
            if ($.isFunction(callbackSim))
                $("#btnSim_" + idModal).click(callbackSim);
            if ($.isFunction(callbackNao))
                $("#btnNao_" + idModal).click(callbackNao);
        }
        $("#" + idModal).modal('show');
    }
};