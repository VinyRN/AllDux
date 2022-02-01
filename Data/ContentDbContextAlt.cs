using System;
using System.Collections.Generic;
using System.Text;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace alldux_plataforma.Data
{
    public class ContentDbContextAlt : DbContext
    {
        public ContentDbContextAlt(DbContextOptions<ContentDbContextAlt> options) : base(options)
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
        
        }
    }
}