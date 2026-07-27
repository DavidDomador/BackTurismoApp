using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class scrollAsientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { "Reporte Reservas", 1, "reporte.read", "/reportes/reservas" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { "Reporte Ventas", 2, "reporte.read", "/reportes/ventas" });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { "Reporte Ventas", 2, "reporte.ventas", "/reportes/ventas" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { "Reporte Salidas", 3, "reporte.salidas", "/reportes/salidas" });

        }
    }
}
