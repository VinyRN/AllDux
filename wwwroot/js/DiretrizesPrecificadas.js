//Javascript de Diretrizes Precificadas
//Tabelas de Diretrizes Precificadas ///////////
function AdicionaRegistro(indice) {
    indiceRegistro = $("#tabelas #tabela_" + indice + " tbody tr").length;
      var linha = `
      <tr class="calculos">
      <td><button type="button" class="btn btn-outline-dark" disabled><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down-up" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M11.5 15a.5.5 0 0 0 .5-.5V2.707l3.146 3.147a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 1 0 .708.708L11 2.707V14.5a.5.5 0 0 0 .5.5zm-7-14a.5.5 0 0 1 .5.5v11.793l3.146-3.147a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 .708-.708L4 13.293V1.5a.5.5 0 0 1 .5-.5z"/>
</svg></button></td>
      <td><input id="ordem" class="form-control tx-w-min" value="" maxlength="20" /></td>
      <td class="text-start">
        <div style="position:relative">
          <span id="NOME_addMedicamento_${indice}_${indiceRegistro}" class="badge bg-secondary visible nomeMedicamento" style="margin-top: -8px; position: absolute; z-index: 10;"></span>
          <div class="input-group" id="popMedicamento" data-bs-toggle="tooltip" data-bs-placement="top" title="Associar Medicamento">
            <input type="text" name="DiretrizPrecificadaRegistro[${indiceRegistro}].Medicamento" class="form-control medicamento" maxlength="150" />
            <button class="btn btn-secondary" type="button" id="addMedicamento_${indice}_${indiceRegistro}" onclick="associarMedicamento(this)" data-indice="${indice}" data-registro="${indiceRegistro}">+</button>
          </div>
        </div>
        <input type="hidden" id="TISS_addMedicamento_${indice}_${indiceRegistro}"  class="tiss" />
      </td>
      <td><input id="Mgm2_${indice}_${indiceRegistro}" maxlength="10" class="form-control tx-w-min mgm2" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" /></td>
      <td><input id="DiasCiclo_${indice}_${indiceRegistro}" maxlength="3" class="form-control tx-w-min diasCiclo" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" /></td>
      <td><input id="ScPeso_${indice}_${indiceRegistro}" maxlength="4" data-indice="${indice}" data-registro="${indiceRegistro}" onfocusout="calculo1(this)" class="form-control tx-w-min ScPeso" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" /></td>
      <td><input id="DoseFinal_${indice}_${indiceRegistro}" class="form-control tx-w-min" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" readonly /></td>
      <td><input id="ValorCpMgAlldux_${indice}_${indiceRegistro}" data-indice="${indice}" data-registro="${indiceRegistro}" class="form-control tx-w-med ValorCpMgAlldux" readonly /></td>
      <td><input id="ValorCiclo_${indice}_${indiceRegistro}" class="form-control tx-w-med" data-indice="${indice}" data-registro="${indiceRegistro}" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" readonly /></td>
      <td><input id="CicloTotal_${indice}_${indiceRegistro}" maxlength="5" data-indice="${indice}" data-registro="${indiceRegistro}" onfocus="calculo2(this)"  onfocusout="calculo3(this)" class="form-control tx-w-min CicloTotal" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" /></td>
      <td><input id="ValorTotal_${indice}_${indiceRegistro}" class="form-control tx-w-med" onkeypress="return event.charCode >= 46 && event.charCode <= 57 && event.charCode != 47" readonly /></td>
      <td><button type="button" class="btn btn-outline-danger" id="removeRegistro" onclick="RemoveRegistro(this)"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
  <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
  <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
</svg></button></td>    
    </tr>
      `;
  
    $("#tabelas #tabela_" + indice + " tbody").append(linha);
    document.getElementById("novoRegistro_" + indice).scrollIntoView();
  }
  
  function AdicionaQuebra(indice) {
    indiceRegistro = $("#tabelas #tabela_" + indice + " tbody tr").length;
      var linha = `
      <tr class="calculos" class="bg-light">
      <td><button type="button" class="btn btn-outline-dark" disabled><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down-up" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M11.5 15a.5.5 0 0 0 .5-.5V2.707l3.146 3.147a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 1 0 .708.708L11 2.707V14.5a.5.5 0 0 0 .5.5zm-7-14a.5.5 0 0 1 .5.5v11.793l3.146-3.147a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 .708-.708L4 13.293V1.5a.5.5 0 0 1 .5-.5z"/>
</svg></button></td>
      <td colspan="10">
      <input type="hidden" id="ordem" value="#quebra#" />
      <input type="text" class="form-control medicamento" value="" />
      </td>
      <td><button type="button" class="btn btn-outline-danger" id="removeRegistro" onclick="RemoveRegistro(this)"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
  <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
  <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
</svg></button></td>    
    </tr>`;
  
    $("#tabelas #tabela_" + indice + " tbody").append(linha);
    document.getElementById("novoRegistro_" + indice).scrollIntoView();
  }
  
  function RemoveRegistro(tr) {
    //$("#tabelas #tabela_" + indice + " tbody tr").remove();
    $(tr).parents('tr').remove();
  }
  
  //Modal de medicamentos
  var addMedModal = new bootstrap.Modal(document.getElementById('addMedicamentoModal'));
  var MedicamentoSelect = null;
  
  function selecionaMedicamento(tiss, nome, valor){
    const sufix = MedicamentoSelect.dataset.indice+"_"+MedicamentoSelect.dataset.registro;
    console.log("Seleciona Medicamento: "+sufix);
    
    //gravar o tiss no campo hidden e mostrar nome do medicamento em algum campo e torna-lo visivel
    $("#TISS_"+MedicamentoSelect.id).val(tiss);
    $("#ValorCpMgAlldux_"+sufix).val(valor.replace(",", "."));
  
    $("#NOME_"+MedicamentoSelect.id).text(nome);
    $("#NOME_"+MedicamentoSelect.id).removeClass('invisible');
    $("#NOME_"+MedicamentoSelect.id).addClass('visible');
  
    addMedModal.hide();
    MedicamentoSelect = null;
  }
  
  function associarMedicamento(e){
    addMedModal.show();
    MedicamentoSelect = e;
  }
  
  //reordenando linhas
  $("#tabela_0 tbody").on("sortupdate", function( event, ui ) {
    console.log("reordenando tabela");
    console.log(ui);
   
  });
  
  //Tabelas de Diretrizes Precificadas ///////////


//Calculos da tabela de diretrizes precificadas //////////////
function calculo1(elemento) {
    let s = "_" + elemento.dataset.indice + "_" + elemento.dataset.registro;
    if (
      $("#Mgm2" + s).val() != "" &&
      $("#DiasCiclo" + s).val() != "" &&
      $("#ScPeso" + s).val() != ""
    ) {
      let resultado = (
        $("#Mgm2" + s).val() *
        $("#DiasCiclo" + s).val() *
        $("#ScPeso" + s).val()
      ).toFixed(2);
      if ($.isNumeric(resultado)) {
        $("#DoseFinal" + s).val(resultado);
        $("#CicloTotal" + s).focus();
      }
    }
  }
  
  function calculo2(elemento) {
    let s = "_" + elemento.dataset.indice + "_" + elemento.dataset.registro;
    console.log("calculo 2: "+s);
    
      let resultado = (
        parseInt($("#DoseFinal" + s).val()) * parseInt($("#ValorCpMgAlldux" + s).val())
      ).toFixed(2);
  
      $("#ValorCiclo" + s).val(resultado);
      $("#CicloTotal" + s).focus();
  }
  
  function calculo3(elemento) {
    let s = "_" + elemento.dataset.indice + "_" + elemento.dataset.registro;
    if (
      $("#CicloTotal" + s).val() != "" &&
      $.isNumeric($("#CicloTotal" + s).val()) &&
      $("#ValorCiclo" + s).val() != "" &&
      $.isNumeric($("#ValorCiclo" + s).val())
    ) {
      let resultado = (
        $("#CicloTotal" + s).val() * $("#ValorCiclo" + s).val()
      ).toFixed(2);
      $("#ValorTotal" + s).val(resultado);
    }
  }
  //Calculos da tabela de diretrizes precificadas //////////////


const element = document.querySelector('form');
element.addEventListener('submit', event => {
    event.preventDefault();
});

function enviaTabela(flag){
    console.log('Preparando tabela para envio');
    $('#gravaTabelaBtn').prop('disabled', true);
    var registros = [];
    var indice = 0;

    $('.calculos').each(function() {
        var registro = {
            "Index" : indice,
            "Ordem" : $(this).find('#ordem').val(),
            "TISS" : $(this).find('.tiss').val(),
            "Medicamento" : $(this).find('.medicamento').val(),
            "Mgm2" : $(this).find('.mgm2').val(),
            "DiasCiclo" : $(this).find('.diasCiclo').val(),
            "ScPeso" : $(this).find('.ScPeso').val(),
            "CicloTotal" : $(this).find('.CicloTotal').val()
        }
        registros.push(registro);
        indice++;
    });

    var tabela = {
        "Id" : $('form').find('#Id').val(),
        "Index" : $('form').find('#Index').val(),
        "Titulo" : $('form').find('.tituloTabela').val(),
        "ChaveTabela" : $('form').find('.chaveTabela').val(),
        "ChaveTabelaReduzida" : $('form').find('.chaveTabelaReduzida').val(),
        "Observacoes" : $('form').find('.campoObs').val(),
        "Linha" : $('form').find('.campoLinha').val(),
        "Finalidade" : $('form').find('.campoFinalidade').val(),
        "DiretrizPrecificadaId" : $('form').find('#DiretrizPrecificadaId').val(),
        "DiretrizPrecificadaRegistro" : registros
    }

    //console.log(tabela);
    if(flag == "create"){
        criaTabela(tabela)
    }else{
        enviaDados(tabela);
    }
}

function enviaDados(tabela){
    console.log("enviando tabela ...");
    $.ajax({
        type: 'POST',
        url: '../EditarTabelaJsonPost',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(tabela),
        traditional: true,
        success: function(result) {
            console.log('Data received: '+result);
            $("#confirma").toast("show");
            $('#gravaTabelaBtn').prop('disabled', false);
        },
        error: function (result) {
            alert("Erro salvando a tabela"+result);
            $('#gravaTabelaBtn').prop('disabled', false);
        }
    });
}

function criaTabela(tabela){
    console.log("enviando tabela ...");
    $.ajax({
        type: 'POST',
        url: '../AdicionarTabelaPost',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(tabela),
        traditional: true,
        success: function(result) {
            console.log('Data received: '+result);
            $("#confirma").toast("show");
            //$('#gravaTabelaBtn').prop('disabled', false);
        },
        error: function (result) {
            alert("Erro salvando a tabela"+result);
        }
    });
}

