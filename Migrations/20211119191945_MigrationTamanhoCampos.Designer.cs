﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using alldux_plataforma.Data;

namespace alldux_plataforma.Migrations
{
    [DbContext(typeof(ContentDbContext))]
    [Migration("20211119191945_MigrationTamanhoCampos")]
    partial class MigrationTamanhoCampos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("alldux_plataforma.Models.BrasindicePF", b =>
                {
                    b.Property<string>("TISS")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApresentacaoCod")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("ApresentacaoNome")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("EAN")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EdicaoAltPreco")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("FlagPisCofins")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Generico")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("IPI")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("LaboratorioCod")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("LaboratorioNome")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MedicamentoCod")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MedicamentoNome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Preco")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PrecoFracionado")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PrecoTipo")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("QtdFracionamento")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("TUSS")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("TISS");

                    b.ToTable("BrasindicePF");
                });

            modelBuilder.Entity("alldux_plataforma.Models.BrasindicePMC", b =>
                {
                    b.Property<string>("TISS")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApresentacaoCod")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("ApresentacaoNome")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("EAN")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EdicaoAltPreco")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("FlagPisCofins")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Generico")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("IPI")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("LaboratorioCod")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("LaboratorioNome")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MedicamentoCod")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("MedicamentoNome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Preco")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PrecoFracionado")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("PrecoTipo")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("QtdFracionamento")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("TUSS")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("TISS");

                    b.ToTable("BrasindicePMC");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizModulo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Conteudo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DiretrizesClinicasId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Index")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("DiretrizesClinicasId");

                    b.ToTable("DiretrizModulo");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificada", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Conteudo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DiretrizesClinicasId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GrupoProtocolo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("DiretrizesClinicasId")
                        .IsUnique();

                    b.ToTable("DiretrizPrecificadas");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificadaRegistro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CicloTotal")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("DiasCiclo")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<Guid>("DiretrizPrecificadaTabelaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Index")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Medicamento")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Mgm2")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Ordem")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ScPeso")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("TISS")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("DiretrizPrecificadaTabelaId");

                    b.ToTable("DiretrizPrecificadaRegistro");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificadaTabela", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChaveTabela")
                        .HasMaxLength(800)
                        .HasColumnType("nvarchar(800)");

                    b.Property<string>("ChaveTabelaReduzida")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("DiretrizPrecificadaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Finalidade")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Index")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Linha")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Observacoes")
                        .HasMaxLength(6000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("DiretrizPrecificadaId");

                    b.ToTable("DiretrizPrecificadaTabela");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizSecao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bibliografia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Conteudo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DiretrizModuloId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Index")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("DiretrizModuloId");

                    b.ToTable("DiretrizSecao");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizesClinicas", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Categoria")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lead")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Tags")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("DiretrizesClinicas");
                });

            modelBuilder.Entity("alldux_plataforma.Models.Faq", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Pergunta")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Resposta")
                        .HasMaxLength(7000)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Faq");
                });

            modelBuilder.Entity("alldux_plataforma.Models.Medicamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descricao")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("DescricaoCurta")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastUpdateUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PrincipioAtivo")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("Id");

                    b.ToTable("Medicamentos");
                });

            modelBuilder.Entity("alldux_plataforma.Models.MedicamentoVariacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Biossimilar")
                        .HasMaxLength(1)
                        .HasColumnType("bit");

                    b.Property<string>("Classe")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Distribuidor")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Familia")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Generico")
                        .HasMaxLength(1)
                        .HasColumnType("bit");

                    b.Property<string>("Laboratorio")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("MedicamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Padronizado")
                        .HasMaxLength(1)
                        .HasColumnType("bit");

                    b.Property<string>("PrecoAlldux")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PrecoMercado")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Referencia")
                        .HasMaxLength(1)
                        .HasColumnType("bit");

                    b.Property<string>("Subclasse")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TISS")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Tipo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UnApresentacao")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UnApresentacaoTipo")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("UnMedida")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("UnMedidaTipo")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentoId");

                    b.ToTable("MedicamentoVariacao");
                });

            modelBuilder.Entity("alldux_plataforma.Models.Negociacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("DestaqueCurto")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DestaqueLongo")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastUpdateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Obs")
                        .HasMaxLength(1600)
                        .HasColumnType("nvarchar(1600)");

                    b.Property<string>("Parceiro")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Texto")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Negociacao");
                });

            modelBuilder.Entity("alldux_plataforma.Models.NegociacaoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MedicamentoId")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<Guid>("NegociacaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PrecoDesconto")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("NegociacaoId");

                    b.ToTable("NegociacaoItem");
                });

            modelBuilder.Entity("alldux_plataforma.Models.TNUMM", b =>
                {
                    b.Property<string>("TISS")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApresentacaoBrasindice")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("CNPJRegistro")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Classe")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("CodAnterior")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Codigo")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("DataFimImplantacao")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DataFimVigencia")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DataInicioVigencia")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DescBrasindice")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("DetentorRegistroAnvisa")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Forma")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Generico")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Grupo")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("MotivoInsercao")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("NomeApresentacao")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Obs")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("PrecoMaxNacional")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PrincipioAtivo")
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("RegistroAnvisa")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("TISSBrasindice")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("TISSCodAnterior")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("TISSTP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxaCustos")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("TipoCodificacao")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TipoProduto")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UnidMinFracao")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ValorFatorConversao")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TISS");

                    b.ToTable("TNUMM");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizModulo", b =>
                {
                    b.HasOne("alldux_plataforma.Models.DiretrizesClinicas", "DiretrizesClinicas")
                        .WithMany("DiretrizModulo")
                        .HasForeignKey("DiretrizesClinicasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiretrizesClinicas");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificada", b =>
                {
                    b.HasOne("alldux_plataforma.Models.DiretrizesClinicas", "DiretrizesClinicas")
                        .WithOne("DiretrizPrecificada")
                        .HasForeignKey("alldux_plataforma.Models.DiretrizPrecificada", "DiretrizesClinicasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiretrizesClinicas");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificadaRegistro", b =>
                {
                    b.HasOne("alldux_plataforma.Models.DiretrizPrecificadaTabela", "DiretrizPrecificadaTabela")
                        .WithMany("DiretrizPrecificadaRegistro")
                        .HasForeignKey("DiretrizPrecificadaTabelaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiretrizPrecificadaTabela");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificadaTabela", b =>
                {
                    b.HasOne("alldux_plataforma.Models.DiretrizPrecificada", "DiretrizPrecificada")
                        .WithMany("DiretrizPrecificadaTabela")
                        .HasForeignKey("DiretrizPrecificadaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiretrizPrecificada");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizSecao", b =>
                {
                    b.HasOne("alldux_plataforma.Models.DiretrizModulo", "DiretrizModulo")
                        .WithMany("DiretrizSecao")
                        .HasForeignKey("DiretrizModuloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiretrizModulo");
                });

            modelBuilder.Entity("alldux_plataforma.Models.MedicamentoVariacao", b =>
                {
                    b.HasOne("alldux_plataforma.Models.Medicamento", "Medicamento")
                        .WithMany("Variacoes")
                        .HasForeignKey("MedicamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicamento");
                });

            modelBuilder.Entity("alldux_plataforma.Models.NegociacaoItem", b =>
                {
                    b.HasOne("alldux_plataforma.Models.Negociacao", "Negociacao")
                        .WithMany("NegociacaoItem")
                        .HasForeignKey("NegociacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Negociacao");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizModulo", b =>
                {
                    b.Navigation("DiretrizSecao");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificada", b =>
                {
                    b.Navigation("DiretrizPrecificadaTabela");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizPrecificadaTabela", b =>
                {
                    b.Navigation("DiretrizPrecificadaRegistro");
                });

            modelBuilder.Entity("alldux_plataforma.Models.DiretrizesClinicas", b =>
                {
                    b.Navigation("DiretrizModulo");

                    b.Navigation("DiretrizPrecificada");
                });

            modelBuilder.Entity("alldux_plataforma.Models.Medicamento", b =>
                {
                    b.Navigation("Variacoes");
                });

            modelBuilder.Entity("alldux_plataforma.Models.Negociacao", b =>
                {
                    b.Navigation("NegociacaoItem");
                });
#pragma warning restore 612, 618
        }
    }
}
