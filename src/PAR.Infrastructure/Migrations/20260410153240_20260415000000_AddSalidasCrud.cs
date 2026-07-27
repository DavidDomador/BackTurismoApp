using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20260415000000_AddSalidasCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salidas",
                columns: table => new
                {
                    iCodSalida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodReserva = table.Column<int>(type: "int", nullable: false),
                    iCodigoGuia = table.Column<int>(type: "int", nullable: false),
                    iCodChoferVehiculo = table.Column<int>(type: "int", nullable: false),
                    fechaSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    horaSalida = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salidas", x => x.iCodSalida);
                    table.ForeignKey(
                        name: "FK_Salidas_ChoferVehiculo_iCodChoferVehiculo",
                        column: x => x.iCodChoferVehiculo,
                        principalTable: "ChoferVehiculo",
                        principalColumn: "iCodigoChoferVehiculo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salidas_Guia_iCodigoGuia",
                        column: x => x.iCodigoGuia,
                        principalTable: "Guia",
                        principalColumn: "iCodigoGuia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salidas_Reservas_iCodReserva",
                        column: x => x.iCodReserva,
                        principalTable: "Reservas",
                        principalColumn: "iCodReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Module", "Name" },
                values: new object[,]
                {
                    { 43, "Ver salidas", "Operaciones", "salida.read" },
                    { 44, "Crear salidas", "Operaciones", "salida.create" },
                    { 45, "Editar salidas", "Operaciones", "salida.update" },
                    { 46, "Eliminar salidas", "Operaciones", "salida.delete" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 43, 1 },
                    { 44, 1 },
                    { 45, 1 },
                    { 46, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_iCodChoferVehiculo",
                table: "Salidas",
                column: "iCodChoferVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_iCodigoGuia",
                table: "Salidas",
                column: "iCodigoGuia");

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_iCodReserva",
                table: "Salidas",
                column: "iCodReserva");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salidas");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 43, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 44, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 45, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 46, 1 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 46);
        }
    }
}
