NovaIntegracao = {

    InitConfiguracao: function () {
        $("input[name='NomeIntegracao']").on('keyup', function (event) {
            let nome = $(this).val();
            if (nome === '') nome = '{nome_integracao}';
            $("label.nome-integracao").text(nome);
        });
        $('#tabsConfiguracao a').on('click', function (event) {
            event.preventDefault();
            $(this).tab('show');
        });
        $('#from-integracao').submit(function(event){
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            $(this).addClass('was-validated');
        });
    },
}
$(function () {
    NovaIntegracao.InitConfiguracao();
});