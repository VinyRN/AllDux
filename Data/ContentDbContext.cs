using alldux_plataforma.Models;
using Microsoft.EntityFrameworkCore;


namespace alldux_plataforma.Data
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
        {

        }
        public DbSet<DiretrizesClinicas> DiretrizesClinicas { get; set; }
        public DbSet<DiretrizModulo> DiretrizModulo { get; set; }
        public DbSet<DiretrizSecao> DiretrizSecao { get; set; }
        public DbSet<DiretrizPrecificada> DiretrizPrecificadas { get; set; }
        public DbSet<DiretrizPrecificadaTabela> DiretrizPrecificadaTabela {get; set; }
        public DbSet<DiretrizPrecificadaRegistro> DiretrizPrecificadaRegistro { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<MedicamentoVariacao> MedicamentoVariacao { get; set; }
        public DbSet<Negociacao> Negociacao { get; set; }
        public DbSet<NegociacaoItem> NegociacaoItem { get; set; }
        public DbSet<Faq> Faq { get; set; }
        public DbSet<BrasindicePMC> BrasindicePMC { get; set; }
        public DbSet<BrasindicePF> BrasindicePF { get; set; }
        public DbSet<TNUMM> TNUMM { get; set; }


        //NovoHub
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ProdutosFavorito> ProdutosFavorito { get; set; }
        public DbSet<Preco> Preco { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<VendaItem> VendaItem { get; set; }
        public DbSet<Fornecedores> Fornecedores { get; set; }
        public DbSet<Frete> Frete{ get; set; }        
        public DbSet<FornecedoresProduto> FornecedoresProduto { get; set; }
        public DbSet<EstadosFederacao> EstadosFederacao { get; set; }
        public DbSet<Paises> Paises { get; set; }
        public DbSet<FornecedoresAnexos> FornecedoresAnexos { get; set; }
        public DbSet<TiposDocFornecedores> TiposDocFornecedores { get; set; }
        public DbSet<FormasPagamento> FormasPagamento { get; set; }
        public DbSet<FornecedoresFormasPag> FornecedoresFormasPag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiretrizesClinicas>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DiretrizModulo>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DiretrizSecao>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DiretrizPrecificada>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DiretrizPrecificadaTabela>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DiretrizPrecificadaRegistro>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Medicamento>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Negociacao>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<NegociacaoItem>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Faq>().Property(x => x.Id).ValueGeneratedOnAdd();

            //NovoHub
            modelBuilder.Entity<Produto>().Property(x => x.Id).ValueGeneratedOnAdd();            
            modelBuilder.Entity<ProdutosFavorito>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Preco>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Venda>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VendaItem>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Fornecedores>().Property(x => x.IdFornecedor).ValueGeneratedOnAdd();
            modelBuilder.Entity<Frete>().Property(x => x.Id_Frete).ValueGeneratedOnAdd();
            modelBuilder.Entity<FornecedoresProduto>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<FornecedoresAnexos>().Property(x => x.IdAnexoFornecedor).ValueGeneratedOnAdd();
            modelBuilder.Entity<TiposDocFornecedores>().Property(x => x.IdTipoDocumento).ValueGeneratedOnAdd();
            modelBuilder.Entity<FormasPagamento>().Property(x => x.IdFormaPagamento).ValueGeneratedOnAdd();
            modelBuilder.Entity<FornecedoresFormasPag>().Property(x => x.IdFormaPagamento).ValueGeneratedOnAdd();

            modelBuilder.Entity<EstadosFederacao>().Property(x => x.IdUf).ValueGeneratedOnAdd();
            modelBuilder.Entity<Paises>().Property(x => x.IdPais).ValueGeneratedOnAdd();
            

            modelBuilder.Entity<DiretrizModulo>()
                    .HasOne(e => e.DiretrizesClinicas)
                    .WithMany(e => e.DiretrizModulo)
                    .HasForeignKey(e => e.DiretrizesClinicasId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiretrizSecao>()
                    .HasOne(e => e.DiretrizModulo)
                    .WithMany(e => e.DiretrizSecao)
                    .HasForeignKey(e => e.DiretrizModuloId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiretrizPrecificada>()
                    .HasOne(e => e.DiretrizesClinicas)
                    .WithOne(e => e.DiretrizPrecificada)
                    .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DiretrizPrecificadaTabela>()
                    .HasOne(e => e.DiretrizPrecificada)
                    .WithMany(e => e.DiretrizPrecificadaTabela)
                    .HasForeignKey(e => e.DiretrizPrecificadaId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiretrizPrecificadaRegistro>()
                    .HasOne(e => e.DiretrizPrecificadaTabela)
                    .WithMany(e => e.DiretrizPrecificadaRegistro)
                    .HasForeignKey(e => e.DiretrizPrecificadaTabelaId)
                    .OnDelete(DeleteBehavior.Cascade);

           modelBuilder.Entity<MedicamentoVariacao>()
                    .HasOne(e => e.Medicamento)
                    .WithMany(e => e.Variacoes)
                    .HasForeignKey(e => e.MedicamentoId)
                    .OnDelete(DeleteBehavior.Cascade);

           modelBuilder.Entity<NegociacaoItem>()
                    .HasOne(e => e.Negociacao)
                    .WithMany(e => e.NegociacaoItem)
                    .HasForeignKey(e => e.NegociacaoId)
                    .OnDelete(DeleteBehavior.Cascade);

          modelBuilder.Entity<BrasindicePMC>()
                    .HasKey(e => e.TISS);
          
          modelBuilder.Entity<BrasindicePF>()
                    .HasKey(e => e.TISS);


         modelBuilder.Entity<FornecedoresAnexos>()
                     .HasOne(e => e.Fornecedor)
                     .WithMany(e => e.Anexos)
                     .HasForeignKey(e => e.IdFornecedor)
                     .OnDelete(DeleteBehavior.Cascade);

         modelBuilder.Entity<FornecedoresProduto>()
                     .HasOne(e => e.Fornecedor)
                     .WithMany(e => e.Produtos)
                     .HasForeignKey(e => e.IdFornecedor)
                     .OnDelete(DeleteBehavior.Cascade);

         modelBuilder.Entity<FornecedoresFormasPag>()
                     .HasOne(e => e.Fornecedor)
                     .WithMany(e => e.FormasPagamento)
                     .HasForeignKey(e => e.IdFornecedor)
                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}