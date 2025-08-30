using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToSaleItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SaleItems",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // Atualizar registros existentes para ter Status = 1 (Active)
            migrationBuilder.Sql("UPDATE \"SaleItems\" SET \"Status\" = 1 WHERE \"Status\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SaleItems");
        }
    }
}
