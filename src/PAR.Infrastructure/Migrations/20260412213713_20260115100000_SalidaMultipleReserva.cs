using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20260115100000_SalidaMultipleReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Reservas_iCodReserva",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_iCodReserva",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "iCodReserva",
                table: "Salidas");

            migrationBuilder.CreateTable(
                name: "SalidaReservas",
                columns: table => new
                {
                    iCodSalidaReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodSalida = table.Column<int>(type: "int", nullable: false),
                    iCodReserva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaReservas", x => x.iCodSalidaReserva);
                    table.ForeignKey(
                        name: "FK_SalidaReservas_Reservas_iCodReserva",
                        column: x => x.iCodReserva,
                        principalTable: "Reservas",
                        principalColumn: "iCodReserva",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalidaReservas_Salidas_iCodSalida",
                        column: x => x.iCodSalida,
                        principalTable: "Salidas",
                        principalColumn: "iCodSalida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalidaReservas_iCodReserva",
                table: "SalidaReservas",
                column: "iCodReserva");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaReservas_iCodSalida",
                table: "SalidaReservas",
                column: "iCodSalida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalidaReservas");

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
