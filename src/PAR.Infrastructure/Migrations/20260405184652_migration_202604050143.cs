using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration_202604050143 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "vSaldo",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VentaDetalles",
                columns: table => new
                {
                    iCodVentaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodVenta = table.Column<int>(type: "int", nullable: false),
                    iCodReservaDetalle = table.Column<int>(type: "int", nullable: false),
                    iCodCliente = table.Column<int>(type: "int", nullable: true),
                    vMonto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalles", x => x.iCodVentaDetalle);
                    table.ForeignKey(
                        name: "FK_VentaDetalles_Clientes_iCodCliente",
                        column: x => x.iCodCliente,
                        principalTable: "Clientes",
                        principalColumn: "iCodCliente");
                    table.ForeignKey(
                        name: "FK_VentaDetalles_ReservaDetalle_iCodReservaDetalle",
                        column: x => x.iCodReservaDetalle,
                        principalTable: "ReservaDetalle",
                        principalColumn: "iCodReservaDetalle"
                        //onDelete: ReferentialAction.Cascade
                        );
                    table.ForeignKey(
                        name: "FK_VentaDetalles_Ventas_iCodVenta",
                        column: x => x.iCodVenta,
                        principalTable: "Ventas",
                        principalColumn: "iCodVenta"
                        //onDelete: ReferentialAction.Cascade
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_iCodCliente",
                table: "VentaDetalles",
                column: "iCodCliente");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_iCodReservaDetalle",
                table: "VentaDetalles",
                column: "iCodReservaDetalle");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_iCodVenta",
                table: "VentaDetalles",
                column: "iCodVenta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VentaDetalles");

            migrationBuilder.DropColumn(
                name: "vSaldo",
                table: "Ventas");
        }
    }
}
