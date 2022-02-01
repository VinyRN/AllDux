using alldux_plataforma.Models;
using Microsoft.EntityFrameworkCore;


namespace alldux_plataforma.Data
{
    public class ContentDbContextHub : DbContext
    {
        public ContentDbContextHub(DbContextOptions<ContentDbContextHub> options) : base(options)
        {

        }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ProdutoVersao> ProdutoVersao { get; set; }
        public DbSet<ProdutosFavorito> ProdutosFavorito { get; set; }
        public DbSet<Preco> Preco { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<VendaItem> VendaItem { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProdutoVersao>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProdutosFavorito>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Preco>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Venda>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VendaItem>().Property(x => x.Id).ValueGeneratedOnAdd();

        }

    }
}
