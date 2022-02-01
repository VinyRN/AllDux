using System;

namespace alldux_plataforma.Models
{
    public class TiposDeAcesso
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public double PrecoAlldux { get; set; }
        public double PrecoComparacao { get; set; }

       public TiposDeAcesso(){}
        public TiposDeAcesso(string id, string nome, double preco, double compara){
            Id = new Guid(id);
            Nome = nome;
            PrecoAlldux = preco;
            PrecoComparacao = compara;
        }
    }
}