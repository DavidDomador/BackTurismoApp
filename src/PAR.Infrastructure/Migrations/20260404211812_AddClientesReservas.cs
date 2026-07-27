using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientesReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    iCodCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cNombres = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    cApellidos = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    cDni = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cCorreo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    cEdad = table.Column<int>(type: "int", nullable: true),
                    cDireccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.iCodCliente);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    iCodReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rNumeroReserva = table.Column<int>(type: "int", nullable: false),
                    iCodPaquete = table.Column<int>(type: "int", nullable: false),
                    fechaTour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    rAbono = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    rSaldo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    rIncluye = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    rNoIncluye = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    rObservacion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.iCodReserva);
                    table.ForeignKey(
                        name: "FK_Reservas_Paquetes_iCodPaquete",
                        column: x => x.iCodPaquete,
                        principalTable: "Paquetes",
                        principalColumn: "iCodPaquete",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { 8, "users2", true, 1, "Clientes", 4, "cliente.read", "/admin/clientes" });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Icon", "IsActive", "Name", "Order" },
                values: new object[] { 4, "reservation", true, "Reserva", 4 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Module", "Name" },
                values: new object[,]
                {
                    { 23, "View clientes", "Cliente", "cliente.read" },
                    { 24, "Create clientes", "Cliente", "cliente.create" },
                    { 25, "Update clientes", "Cliente", "cliente.update" },
                    { 26, "Delete clientes", "Cliente", "cliente.delete" },
                    { 27, "View reservas", "Reserva", "reserva.read" },
                    { 28, "Create reservas", "Reserva", "reserva.create" },
                    { 29, "Update reservas", "Reserva", "reserva.update" },
                    { 30, "Delete reservas", "Reserva", "reserva.delete" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Icon", "IsActive", "MenuId", "Name", "Order", "RequiredPermission", "Route" },
                values: new object[] { 9, "calendar", true, 4, "Reservas", 1, "reserva.read", "/reserva/reservas" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 23, 1 },
                    { 24, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_iCodPaquete",
                table: "Reservas",
                column: "iCodPaquete");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_rNumeroReserva",
                table: "Reservas",
                column: "rNumeroReserva",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 26, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 27, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 29, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 30, 1 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
