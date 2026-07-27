using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaquetePdfAdjunto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pdfAdjunto",
                table: "Paquetes",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pdfAdjunto",
                table: "Paquetes");
        }
    }
}
