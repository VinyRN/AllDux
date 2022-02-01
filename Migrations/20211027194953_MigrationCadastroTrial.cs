using Microsoft.EntityFrameworkCore.Migrations;

namespace alldux_plataforma.Migrations
{
    public partial class MigrationCadastroTrial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoseFinal",
                table: "DiretrizPrecificadaRegistro");

            migrationBuilder.DropColumn(
                name: "ValorCiclo",
                table: "DiretrizPrecificadaRegistro");

            migrationBuilder.DropColumn(
                name: "ValorCpMgAlldux",
                table: "DiretrizPrecificadaRegistro");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "DiretrizPrecificadaRegistro");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ApplicationUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeGestor",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelGestor",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Cargo",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "NomeGestor",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "TelGestor",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<string>(
                name: "DoseFinal",
                table: "DiretrizPrecificadaRegistro",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValorCiclo",
                table: "DiretrizPrecificadaRegistro",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValorCpMgAlldux",
                table: "DiretrizPrecificadaRegistro",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValorTotal",
                table: "DiretrizPrecificadaRegistro",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
