EditarOperacoes = {

    InitConfiguracao: function () {

        $('.adicionar-atributo').click(function() {
            const atributosIndex = $('#container-atributos').children().length;
            $('#container-atributos').append(`
                <div class="field-group" data-index="${atributosIndex}">
                    <div class="row g-4">
                        <div class="col-auto mt-5">
                            <a href="#" class="mover-atributo fs-2" title="Mover atributo"><i class="icon ion-grid"></i></a>
                        </div>
                        <div class="col-8">
                            <input type="text" class="form-control" id="nomeAtributo${atributosIndex}" name="nomeAtributo" required/>
                        </div>
                        <div class="col-3">
                            <select class="form-select" id="tipoAtributo${atributosIndex}" name="tipoAtributo" required>
                                <option value="alfanumerico">Texto</option>
                                <option value="numerico">Número</option>
                                <option value="objeto">Objeto</option>
                            </select>
                        </div>
                        <div class="col-auto mt-5">
                            <a href="#" class="remover-atributo fs-2" data-index="${atributosIndex}" title="Remover atributo"><i class="icon ion-trash-a"></i></a>
                        </div>
                    </div>
                </div>
            `);
            EditarOperacoes.InitRemoverEMoverAtributos();
            
        });
    },
    InitRemoverEMoverAtributos: function () {
        $('.remover-atributo').click(function() {
            var index = $(this).data('index');
            console.log(index);
        });
        
        $('#container-atributos').sortable({
            update: function(event, ui) {
                updateIndices();
            }
        });
        function updateIndices() {
            $('#container-atributos .field-group').each(function(index) {
                $(this).attr('data-index', index);
                $(this).find('.form-control').each(function() {
                    var id = $(this).attr('id');
                    var newId = id.split('_')[0] + '_' + index;
                    $(this).attr('id', newId);
                });
            });
        }        
    },
}
$(function () {
    EditarOperacoes.InitConfiguracao();
});