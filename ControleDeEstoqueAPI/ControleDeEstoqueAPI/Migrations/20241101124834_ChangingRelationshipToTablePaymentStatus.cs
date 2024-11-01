using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoqueAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangingRelationshipToTablePaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStatuses_PaymentStatusId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatusId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatusId",
                table: "Orders",
                column: "PaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentStatuses_PaymentStatusId",
                table: "Orders",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuses",
                principalColumn: "PaymentStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStatuses_PaymentStatusId",
                table: "Payments",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuses",
                principalColumn: "PaymentStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentStatuses_PaymentStatusId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStatuses_PaymentStatusId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentStatusId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatusId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStatuses_PaymentStatusId",
                table: "Payments",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuses",
                principalColumn: "PaymentStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
