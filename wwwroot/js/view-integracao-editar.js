EditarIntegracao = {
    InitConfiguracao: function () {
        $("input[name='NomeIntegracao']").on('keyup', function (event) {
            let nome = $(this).val();
            if (nome === '') nome = '{nome_integracao}';
            $("label.nome-integracao").text(nome);
        });
        $('#tabsConfiguracao a').on('click', function (event) {
            var idIntegracao = $("#IdIntegracao").val();
            var nome_tab = $(this).data('tab');
            if ('tab_operacoes' === nome_tab) {
                OperacaoIntegracao.NovaOperacao(idIntegracao);
            } else if ('tab_authorization' === nome_tab) {
                AuthorizationIntegracao.ObterAuthorization();
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
}
$(function () {
    EditarIntegracao.InitConfiguracao();
});