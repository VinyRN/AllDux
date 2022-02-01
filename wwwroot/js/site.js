$(document).ready(function () {
  //Affix
  var toggleAffix = function (affixElement, scrollElement, wrapper) {
    var height = affixElement.outerHeight(),
    top = wrapper.offset().top;

    if (scrollElement.scrollTop() >= top) {
      wrapper.height(height);
      affixElement.addClass("affix");
      affixElement.width(affixElement.parent().innerWidth() - 30);
    } else {
      affixElement.removeClass("affix");
      wrapper.height("auto");
    }
  };

  if ($('[data-toggle="affix"]').length) {
    $('[data-toggle="affix"]').each(function () {
      var ele = $(this),
        wrapper = $("<div></div>");

      ele.before(wrapper);
      $(window).on("scroll resize", function () {
        toggleAffix(ele, $(this), wrapper);
      });

      // init
      toggleAffix(ele, $(window), wrapper);
      //Affix
    });
  }

  //inicializando tooltips
  var tooltipTriggerList = [].slice.call(
    document.querySelectorAll('[data-bs-toggle="tooltip"]')
  );
  var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
  });

  $(function () {
    $('[data-toggle="tooltip"]').tooltip();
  });

  //inicializando textareas
  //nicEditors.allTextAreas({buttonList : ['fontSize','bold','italic','underline','strikethrough','subscript','superscript', 'left', 'center', 'right', 'justify', 'link', 'unlink', 'image'], iconsPath : '/img/nicEditorIcons.gif' });
  nicEditors.allTextAreas({
    fullPanel: true,
    iconsPath: "/img/nicEditorIcons.gif",
  });

  //auto hide alert
  if ($(".alert-dismissible").length != 0) {
    console.log("alerta ativo");
    $(".alert-dismissible")
      .fadeTo(5000, 500)
      .slideUp(500, function () {
        $("#alerta").slideUp(500);
      });
  }

  //inicializando Datepicker
  $("#sandbox-container input").datepicker({
    format: "dd/mm/yyyy",
    language: "pt-BR",
    todayHighlight: true,
  });

  //DataTable da tabela de usuarios
  if ($("#admin-index").length) {
    $("#admin-index").DataTable({
      language: { url: "/js/Portuguese-Brasil.json" },
    });
  }

  //DataTable da tabela de Medicamentos
  if ($("#Medicamentos").length) {
    $("#Medicamentos").DataTable({
      language: { url: "/js/Portuguese-Brasil.json" },
    });
  }

  //DataTable da tabela de Negociacoes
  if ($("#negociacoes").length) {
    $("#negociacoes").DataTable({
      language: { url: "/js/Portuguese-Brasil.json" },
    });
  }

  if ($("#TabelaMedicamentosNegociacoes").length) {
    $("#TabelaMedicamentosNegociacoes").DataTable({
      paging: false,
      scrollY: 650,
      language: { url: "/js/Portuguese-Brasil.json" },
    });
  }
  
  if ($("#TabelaMedicamentosDiretrizes").length) {
    $("#TabelaMedicamentosDiretrizes").DataTable({
      paging: false,
      scrollY: 300,
      language: { url: "/js/Portuguese-Brasil.json" },
    });
  }
  
  //Auto completar das categorias de Diretrizes
  if (typeof arrayCategoria != "undefined") {
    $("#Categoria").autocomplete({ source: arrayCategoria });
  }

  if (typeof arrayLinha != "undefined") {
    $("#linha").autocomplete({ source: arrayLinha });
  }

  if (typeof arrayFinalidade != "undefined") {
    $("#finalidade").autocomplete({ source: arrayFinalidade });
  }

  if (typeof modal !== "undefined") {
    //console.log("Modal acionada");
    $("#PoliticaPrivacidade").modal("show");
  }

  //oculta o simbolo de loading
  $("#loading").hide();

  //plugin anti viuvas
  $(".card-diretriz-titulo").widowFix();

  //arrastar e soltar das tabelas de precificadas
  $(".tabelaDrop tbody").sortable().disableSelection();

}); //Documment.Ready

//Adicionar Diretriz ///////////
function AdicionaModulo() {
  var i = $("#modulos > div").length;
  var modulo = `<div id="modulo${i}" class="pb-3" style="margin-left:20px;">
        <input type="hidden" name="DiretrizModulo[${i}].Index" value="${i}" maxlength="3" />
        <div class="form-group mb-3">
        <label asp-for="Titulo">Título do Módulo <button type="button" class="btn btn-outline-danger btn-sm" id="addItem" onclick="RemoveItem(modulo${i})">Remove Módulo</button></label>
        <input asp-for="Titulo" name="DiretrizModulo[${i}].Titulo" id="DiretrizModulo_${i}" class="form-control" />
        </div>
        <div class="form-group mb-3">
        <label asp-for="Conteudo">Conteúdo do Módulo</label>
        <textarea asp-for="Conteudo" name="DiretrizModulo[${i}].Conteudo" id="Textrea_${i}" style="width:100%; height:250px; display:block;"></textarea>
        </div>
        <div id="secoes${i}"></div>
        <button type="button" class="btn btn-secondary" id="addItem" onclick="AdicionaSecao(${i})">Adicionar nova Seção</button>
        </div>`;

  $("#modulos").append(modulo);
  new nicEditor({
    buttonList: [
      "fontSize",
      "bold",
      "italic",
      "underline",
      "strikethrough",
      "subscript",
      "superscript",
      "left",
      "center",
      "right",
      "justify",
      "link",
      "unlink",
      "image",
    ],
    iconsPath: "/img/nicEditorIcons.gif",
  }).panelInstance("Textrea_" + i);
  $("#DiretrizModulo_" + i).focus();
  document.getElementById("modulo" + i).scrollIntoView();
}

function AdicionaSecao(indice) {
  j = $("#secoes" + indice + " > div").length;
  var secao = `
        <div id="secao${j}_${indice}" class="pb-3" style="margin-left:20px;">
          <input type="hidden" name="DiretrizModulo[${indice}].DiretrizSecao[${j}].Index" value="${j}" maxlength="3" />
          <div class="form-group mb-3">
            <label asp-for="Titulo">Título da Seção <button type="button" class="btn btn-outline-danger btn-sm" id="addItem" onclick="RemoveItem(secao${j}_${indice})">Remove Seção</button></label>
            <input asp-for="Titulo" name="DiretrizModulo[${indice}].DiretrizSecao[${j}].Titulo" id="DiretrizSecao_${indice}_${j}" class="form-control" />
          </div>
          <div class="form-group mb-3">
            <label asp-for="Conteudo">Conteúdo</label>
            <textarea asp-for="Conteudo" name="DiretrizModulo[${indice}].DiretrizSecao[${j}].Conteudo" id="Textrea_${j}_${indice}" style="width:100%; height:250px; display:block;"></textarea>
          </div>
          <div class="form-group mb-3">
            <label asp-for="Bibliografia">Bibliografia</label>
            <input asp-for="Bibliografia" name="DiretrizModulo[${indice}].DiretrizSecao[${j}].Bibliografia" class="form-control" />
          </div>
          <div class="form-group mb-3">
            <label asp-for="Observacoes">Observações</label>
            <textarea asp-for="Observacoes" name="DiretrizModulo[${indice}].DiretrizSecao[${j}].Observacoes" id="Textrea_obs_${j}_${indice}" style="width:100%; height:150px; display:block;"></textarea>
          </div>
        </div>`;

  $("#modulo" + indice + " #secoes" + indice).append(secao);
  new nicEditor({
    buttonList: ["fontSize", "bold","italic","underline","strikethrough","subscript","superscript","left","center","right","justify","link","unlink","image",],
    iconsPath: "/img/nicEditorIcons.gif",
  }).panelInstance("Textrea_" + j + "_" + indice);
  new nicEditor({ buttonList: ["fontSize","bold","italic","underline","strikethrough","subscript","superscript","left","center","right","justify","link","unlink","image",],
    iconsPath: "/img/nicEditorIcons.gif",
  }).panelInstance("Textrea_obs_" + j + "_" + indice);
  $("#DiretrizSecao_" + indice + "_" + j).focus();
  document.getElementById("secao" + j + "_" + indice).scrollIntoView();
}

function RemoveItem(secao) {
  $(secao).remove();
  console.log("remove item " + secao);
}
//Adicionar Diretriz ///////////

//Filtro de categorias de Diretrizes ///////////
function filtraCategoria(Filter) {
  $("div.diretriz").show();
  $("div.diretriz:not([data-categoria='" + Filter + "'])").hide();
  if (Filter == "") {
    $("div.diretriz").show();
  }

  reorganizar();
}

// busca-diretrizes //////////////
$("#busca-diretrizes").on("keyup", function () {
  var input = $(this).val().toUpperCase();
  console.log(input);
  $(".diretrizRow .diretriz").each(function () {
    if ($(this).text().toUpperCase().indexOf(input) < 0) {
      $(this).hide();
    } else {
      $(this).show();
    }
  });

  reorganizar();
});

function reorganizar() {
  var largura = $(".diretrizRow").width() / 3;
  $("#icon-grid .row").masonry({
    columnWidth: largura,
    itemSelector: ".diretriz",
    percentPosition: true,
  });
}
//Filtro de categorias de Diretrizes ///////////

//Medicamentos - Variações ///////////
function AdicionaVariacao() {
  elem = $("#variacoes > div");
  i = elem.length;
  var variacao = `
    <div id="variacao_${i}" class="variacao p-3 mb-2">
      <div class="row">
        <div class="col mb-3">
          <label name="Variacoes[${i}].Nome">Nome</label>
          <input name="Variacoes[${i}].Nome" class="form-control" />
        </div>
        <div class="col mb-3">
          <label name="Variacoes[${i}].TISS">TISS</label>
          <input name="Variacoes[${i}].TISS" class="form-control" />
        </div>
        <div class="col mb-3">
          <label name="Variacoes[${i}].Laboratorio">Laboratorio</label>
          <input name="Variacoes[${i}].Laboratorio" class="form-control" />
        </div>
        <div class="col mb-3">
          <label name="Variacoes[${i}].Distribuidor">Distribuidor</label>
          <input name="Variacoes[${i}].Distribuidor" class="form-control" />
        </div>
      </div>

      <div class="row">
      <div class="col mb-3">
          <label name="Variacoes[${i}].Tipo">Tipo</label>
          <input name="Variacoes[${i}].Tipo" class="form-control" />
        </div>
          <div class="col mb-3">
              <label name="Variacoes[${i}].Familia">Família</label>
              <input type="text" name="Variacoes[${i}].Familia" class="form-control" />
          </div>
          <div class="col mb-3">
              <label name="Variacoes[${i}].Classe">Classe</label>
              <input  type="text" name="Variacoes[${i}].Classe" class="form-control" />
          </div>
          <div class="col mb-3">
              <label name="Variacoes[${i}].Subclasse">Subclasse</label>
              <input  type="text" name="Variacoes[${i}].Subclasse" class="form-control" />
          </div>
      </div>

      <div class="row">

      <div class="col mb-3">
          <label name="Variacoes[${i}].UnMedida" data-bs-toggle="tooltip" data-bs-placement="top" title="Quantidade na Embalagem">Quantidade</label>
          <div class="input-group mb-3">
            <input type="text" name="Variacoes[${i}].UnMedida" class="form-control" />
            <input list="UnMedida" class="form-control" name="Variacoes[${i}].UnMedidaTipo" />
            <datalist id="UnMedida">
              <option value="Cp"></option>
              <option value="Caps"></option>
              <option value="Amp"></option>
            </datalist>
          </div>
        </div>

        <div class="col mb-3">
          <label name="Variacoes[${i}].UnApresentacao" data-bs-toggle="tooltip" data-bs-placement="top" title="Dose da unidade da embalagem">Posologia</label>
          <div class="input-group mb-3">
            <input type="text" name="Variacoes[${i}].UnApresentacao" class="form-control" />
            <input list="UnApresentacao" class="form-control" name="Variacoes[${i}].UnApresentacaoTipo" />
            <datalist id="UnApresentacao">
              <option value="mg"></option>
              <option value="ml"></option>
            </datalist>
          </div>
        </div>

        <div class="col mb-3">
          <label name="Variacoes[${i}].PrecoMercado">Preço de Mercado</label>
          <div class="input-group mb-3">
            <span class="input-group-text">R$</span>
            <input type="text" name="Variacoes[${i}].PrecoMercado" class="form-control" />
          </div>
        </div>

        <div class="col mb-3">
          <label name="Variacoes[${i}].PrecoAlldux">Preço Alldux</label>
          <div class="input-group mb-3">
            <span class="input-group-text">R$</span>
            <input type="text" name="Variacoes[${i}].PrecoAlldux" class="form-control" />
          </div>
        </div>

      </div>

      <div class="row">
          <div class="col mb-3 ps-4">
              <div class="form-check form-switch mt-2">
                  <input class="form-check-input btn-lg" type="checkbox" id="Padronizado_${i}" name="Padronizado_${i}" onclick="document.getElementById('Variacoes[${i}].Padronizado').value = this.checked" />
                  <label class="form-check-label" for="Padronizado">Padronizado</label>
                  <input type="hidden" value="false" name="Variacoes[${i}].Padronizado" id="Variacoes[${i}].Padronizado" />
              </div>
          </div>

          <div class="col mb-3 ps-4">
              <div class="form-check form-switch mt-2">
                  <input class="form-check-input btn-lg" type="checkbox" id="Generico_${i}" name="Generico_${i}" onclick="document.getElementById('Variacoes[${i}].Generico').value = this.checked" />
                  <label class="form-check-label" for="Generico">Genérico</label>
                  <input type="hidden" value="false" name="Variacoes[${i}].Generico" id="Variacoes[${i}].Generico" />
              </div>
          </div>

          <div class="col mb-3 ps-4">
              <div class="form-check form-switch mt-2">
                  <input class="form-check-input btn-lg" type="checkbox" id="Biossimilar_${i}" name="Biossimilar_${i}" onclick="document.getElementById('Variacoes[${i}].Biossimilar').value = this.checked" />
                  <label class="form-check-label" for="Biossimilar">Biossimilar</label>
                  <input type="hidden" value="false" name="Variacoes[${i}].Biossimilar" id="Variacoes[${i}].Biossimilar" />
              </div>
          </div>

          <div class="col mb-3 ps-4">
              <div class="form-check form-switch mt-2">
                  <input class="form-check-input btn-lg" type="checkbox" id="Referencia_${i}" name="Referencia_${i}" onclick="document.getElementById('Variacoes[${i}].Referencia').value = this.checked" />
                  <label class="form-check-label" for="Referencia">Referência</label>
                  <input type="hidden" value="false" name="Variacoes[${i}].Referencia" id="Variacoes[${i}].Referencia" />
              </div>
          </div>
      </div>
      
    </div><!-- variacao_${i} -->`;

  $("#variacoes").append(variacao);
  document.getElementById("addItem").scrollIntoView();
}

function RemoveVariacao() {
  i = $("#variacoes > div").length - 1;
  $("#variacoes #variacao_" + i).remove();
  console.log("variacao excluida: " + i);
}
//Medicamentos - Variações ///////////

// Adicionar item na negociacao
function negociacaoItem_CarregaId(orig, elem, id) {
  console.log("togle negociacao > " + orig.checked);
  if (orig.checked == true) {
    document.getElementById(elem).value = id;
  } else {
    document.getElementById(elem).value = "";
  }
}

// Exibe Loading //////////////////////
function ExibeLoading() {
  $("#loading").show();
}

// botao topo ////////////////////////
mybutton = document.getElementById("btTopo");
window.onscroll = function () {
  scrollFunction();
};

function scrollFunction() {
  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    mybutton.style.display = "block";
  } else {
    mybutton.style.display = "none";
  }
}

function topFunction() {
  document.body.scrollTop = 0; // For Safari
  document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

