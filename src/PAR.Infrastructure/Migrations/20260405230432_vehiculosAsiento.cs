using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vehiculosAsiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "gCantidadAsientos",
                table: "Vehiculos",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Icon", "IsActive", "Name", "Order" },
                values: new object[] { 6, "reportes", true, "Reportes", 6 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Module", "Name" },
                values: new object[,]
                {
                    { 39, "View reporte reservas", "Reportes", "reporte.reservas" },
                    { 40, "View reporte ventas", "Reportes", "reporte.ventas" },
                    { 41, "View reporte salidas", "Reportes", "reporte.salidas" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[,]
                {
                    { 12, "chart-bar", true, 6, "Reporte Reservas", 1, "reporte.reservas", "/reportes/reservas" },
                    { 13, "chart-bar", true, 6, "Reporte Ventas", 2, "reporte.ventas", "/reportes/ventas" },
                    { 14, "chart-bar", true, 6, "Reporte Salidas", 3, "reporte.salidas", "/reportes/salidas" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 39, 1 },
                    { 40, 1 },
                    { 41, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 39, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 40, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 41, 1 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DropColumn(
                name: "gCantidadAsientos",
                table: "Vehiculos");
        }
    }
}
