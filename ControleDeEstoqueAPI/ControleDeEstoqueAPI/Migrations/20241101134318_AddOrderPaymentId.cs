using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoqueAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderPaymentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderPaymentId",
                table: "OrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPaymentId",
                table: "OrderPayments");
        }
    }
}
