using Microsoft.EntityFrameworkCore.Migrations;

namespace alldux_plataforma.Migrations
{
    public partial class MigrationPrec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChaveTabelaReduzida",
                table: "DiretrizPrecificadaTabela",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChaveTabelaReduzida",
                table: "DiretrizPrecificadaTabela");
        }
    }
}
