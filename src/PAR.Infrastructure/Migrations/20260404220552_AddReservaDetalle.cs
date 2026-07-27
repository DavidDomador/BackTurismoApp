using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservaDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservaDetalle",
                columns: table => new
                {
                    iCodReservaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodReserva = table.Column<int>(type: "int", nullable: false),
                    iCodCliente = table.Column<int>(type: "int", nullable: false),
                    estadoCliente = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaDetalle", x => x.iCodReservaDetalle);
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_Clientes_iCodCliente",
                        column: x => x.iCodCliente,
                        principalTable: "Clientes",
                        principalColumn: "iCodCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_DefinicionDetalle_estadoCliente",
                        column: x => x.estadoCliente,
                        principalTable: "DefinicionDetalle",
                        principalColumn: "idDefD");
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_Reservas_iCodReserva",
                        column: x => x.iCodReserva,
                        principalTable: "Reservas",
                        principalColumn: "iCodReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_estadoCliente",
                table: "ReservaDetalle",
                column: "estadoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_iCodCliente",
                table: "ReservaDetalle",
                column: "iCodCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_iCodReserva",
                table: "ReservaDetalle",
                column: "iCodReserva");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaDetalle");
        }
    }
}
