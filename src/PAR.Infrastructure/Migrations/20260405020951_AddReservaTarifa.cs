using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservaTarifa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "rTarifa",
                table: "Reservas",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rTarifa",
                table: "Reservas");
        }
    }
}
