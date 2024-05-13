Integracao = {

    InitConfiguracao: function () {
        $("input[name='nomeIntegracao']").on('keyup', function (event) {
            let nome = $(this).val();
            if (nome === '')
                nome = '{nome_integracao}';
            $("label.nome-integracao").text(nome);
        });
        $('#tabsConfiguracao a').on('click', function (event) {
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
}
$(function () {
    Integracao.InitConfiguracao();
});