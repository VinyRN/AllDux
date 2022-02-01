using System.Collections.Generic;

namespace alldux_plataforma.Models
{
    public class ProdutoHubViewModels
    {

        public Produto objPrduto { get; set; }
        public Medicamento objMedicamento { get; set; }
        public ProdutoFornecedorHub ObjProdutoVerHub { get; set; }

        public List<Medicamento> objListMedicamento { get; set; }
        public List<Produto> objListProduto { get; set; }
        public List<ProdutoFornecedorHub> objListProdutoVerHub { get; set; }
        

    }
}
