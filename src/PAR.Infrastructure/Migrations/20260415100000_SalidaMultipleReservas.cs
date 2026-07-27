using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    public partial class SalidaMultipleReservas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Quitar FK e índice de la columna iCodReserva en Salidas
            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Reservas_iCodReserva",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_iCodReserva",
                table: "Salidas");

            // 2. Quitar columna iCodReserva de Salidas
            migrationBuilder.DropColumn(
                name: "iCodReserva",
                table: "Salidas");

            // 3. Crear tabla junction SalidaReservas
            migrationBuilder.CreateTable(
                name: "SalidaReservas",
                columns: table => new
                {
                    iCodSalidaReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodSalida  = table.Column<int>(type: "int", nullable: false),
                    iCodReserva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaReservas", x => x.iCodSalidaReserva);
                    table.ForeignKey(
                        name: "FK_SalidaReservas_Salidas_iCodSalida",
                        column: x => x.iCodSalida,
                        principalTable: "Salidas",
                        principalColumn: "iCodSalida",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalidaReservas_Reservas_iCodReserva",
                        column: x => x.iCodReserva,
                        principalTable: "Reservas",
                        principalColumn: "iCodReserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalidaReservas_iCodSalida",
                table: "SalidaReservas",
                column: "iCodSalida");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaReservas_iCodReserva",
                table: "SalidaReservas",
                column: "iCodReserva");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SalidaReservas");

            migrationBuilder.AddColumn<int>(
                name: "iCodReserva",
                table: "Salidas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_iCodReserva",
                table: "Salidas",
                column: "iCodReserva");

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Reservas_iCodReserva",
                table: "Salidas",
                column: "iCodReserva",
                principalTable: "Reservas",
                principalColumn: "iCodReserva",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
