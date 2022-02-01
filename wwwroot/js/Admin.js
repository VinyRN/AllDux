// Trial /////////////////////////////////

function ativaUser(elem){
  var ativo = $(elem).prop("checked");
  var userId =  $(elem).data('id');
  var data = {
    "Id" : userId,
    "Active" : ativo
  }
  enviaDadosJson(data, "/Admin/UserActive");
}

function enviaDadosJson(ajaxData, url){
  console.log("enviando dados ...");
  $.ajax({
      type: 'POST',
      url: url,
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      data: JSON.stringify(ajaxData),
      traditional: true,
      success: function(result) {
          console.log('Dados salvos: '+result);
      },
      error: function (result) {
          console.log("Erro salvando dados"+result);
      }
  });
}