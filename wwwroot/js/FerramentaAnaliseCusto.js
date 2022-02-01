// Ferramenta de Analise de Custo /////////////////////////////////

$(document).ready(function () {
  var myModal = new bootstrap.Modal(document.getElementById('OnLoad'), {})
  myModal.show();
});


// Ferramentas de Analises ////////////////////////
function calculaSC() {
  const iAltura = document.getElementById("altura");
  const iPeso = document.getElementById("peso");
  const iSC = document.getElementById("sc");

  if (iPeso.value != "" && iAltura.value != "") {
    const total =
      0.007184 * Math.pow(iAltura.value, 0.725) * Math.pow(iPeso.value, 0.425);
      iSC.value = total.toFixed(2);
  } else {
    iSC.value = "";
  }
}

function carregaSelectTabela(obj) {
  var val = obj.value;
  var nome = obj.selectedOptions[0].textContent;

  if (val != "") {
    $("[name='DiretrizNome']").val(nome);
    $.post(
      "CarregaSelectTabela",
      {
        Id: val,
      },
      function (lista) {
        $("#SelectTabela").prop("disabled", false);
        $("#SelectTabela").find("option").remove();
        $("<option>")
          .val("")
          .text("Selecione ...")
          .appendTo($("#SelectTabela"));

        for (var i = 0; i < lista.length; i++) {
          $("<option>")
            .val(lista[i].key)
            .text(lista[i].value)
            .appendTo($("#SelectTabela"));
        }
      }
    );
  } else {
    $("#SelectTabela").prop("disabled", true);
    $("#SelectTabela").find("option").remove();
    $("<option>")
      .val("")
      .text("")
      .appendTo($("#SelectTabela"));
  }
}

function preencheAUC(){
  $("#auc").val($("#AUC-resultado").val());
}

$("#CalculoAUC").submit(function(e){
  e.preventDefault();
  console.log("Calcula AUC");

  //((140 - A) * W / (72 * Cr)
  //woman: 0.85 * formula
  
  const auc = $("#AUC-Alvo").val();
  const genero = $("#AUC-genero").val();
  const idade = $("#AUC-idade").val();
  const peso = $("#AUC-peso").val();
  const creatina = $("#AUC-creatina").val();

  const gfr = genero * ((140 - idade) * peso / (72 * creatina));

  const resultado = auc * (gfr + 25);

  $("#AUC-resultado").val(Math.round(resultado));

})

$("#CalculoCusto").submit(function(e){
  e.preventDefault();
  console.log("calculo Custo Operacional")
  
  const result = $("#TotalCusto").val() / $("#TotalTratamentos").val();
  
  if(!isNaN(result)) $("#Custo-resultado").val(result.toFixed(2));
});

function preencheCusto(){
  var ModalCalculaCusto = new bootstrap.Modal(document.getElementById('CustoOperacional'));
  $("#CustoOperacionalCalculado").val($("#Custo-resultado").val());
  ModalCalculaCusto.hide();
}

function selecionaComparacao(i){
  $("#brasindice").val(i);
  
  if(i == "PMC"){
    $("#PercentPF").removeClass('visible');
    $("#PercentPF").addClass('invisible');
  }else if(i == "PFB"){
    $("#PercentPF").removeClass('invisible');
    $("#PercentPF").addClass('visible');
  }else if(i == "TNUMM"){
    $("#PercentPF").removeClass('visible');
    $("#PercentPF").addClass('invisible');
  }
}