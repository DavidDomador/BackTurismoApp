using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItinerarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itinerario",
                columns: table => new
                {
                    iCodItinerario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCodPaquete = table.Column<int>(type: "int", nullable: false),
                    iNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    iDescripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    imagen = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itinerario", x => x.iCodItinerario);
                    table.ForeignKey(
                        name: "FK_Itinerario_Paquetes_iCodPaquete",
                        column: x => x.iCodPaquete,
                        principalTable: "Paquetes",
                        principalColumn: "iCodPaquete",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itinerario_iCodPaquete",
                table: "Itinerario",
                column: "iCodPaquete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itinerario");
        }
    }
}
