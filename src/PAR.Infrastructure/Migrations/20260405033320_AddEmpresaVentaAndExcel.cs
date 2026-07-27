using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpresaVentaAndExcel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    iCodEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eRazonSocial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    eRUC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    eDireccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    eTelefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    eCorreo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    eRepresentante = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.iCodEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    iCodVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vNumeroVenta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    iCodReserva = table.Column<int>(type: "int", nullable: false),
                    vFechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    iCodUsuario = table.Column<int>(type: "int", nullable: false),
                    vUsuarioNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    iCodCliente = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.iCodVenta);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes_iCodCliente",
                        column: x => x.iCodCliente,
                        principalTable: "Clientes",
                        principalColumn: "iCodCliente");
                    table.ForeignKey(
                        name: "FK_Ventas_Reservas_iCodReserva",
                        column: x => x.iCodReserva,
                        principalTable: "Reservas",
                        principalColumn: "iCodReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { 10, "building", true, 1, "Empresa", 5, "empresa.read", "/admin/empresa" });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Icon", "IsActive", "Name", "Order" },
                values: new object[] { 5, "venta", true, "Venta", 5 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Module", "Name" },
                values: new object[,]
                {
                    { 31, "View empresas", "Empresa", "empresa.read" },
                    { 32, "Create empresas", "Empresa", "empresa.create" },
                    { 33, "Update empresas", "Empresa", "empresa.update" },
                    { 34, "Delete empresas", "Empresa", "empresa.delete" },
                    { 35, "View ventas", "Venta", "venta.read" },
                    { 36, "Create ventas", "Venta", "venta.create" },
                    { 37, "Update ventas", "Venta", "venta.update" },
                    { 38, "Delete ventas", "Venta", "venta.delete" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { 11, "receipt", true, 5, "Ventas", 1, "venta.read", "/venta/ventas" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 36, 1 },
                    { 37, 1 },
                    { 38, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_iCodCliente",
                table: "Ventas",
                column: "iCodCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_iCodReserva",
                table: "Ventas",
                column: "iCodReserva");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 31, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 32, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 33, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 34, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 35, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 36, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 37, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 38, 1 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 38);
        }
    }
}
