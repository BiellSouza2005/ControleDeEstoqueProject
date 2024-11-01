using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoqueAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderProductId",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderProductId",
                table: "OrderProducts");
        }
    }
}
