using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoqueAPI.Migrations
{
    public partial class AddDescriptionIDToProductDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Removendo a chave primária existente na tabela ProductDescriptions
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'PK_ProductDescription' AND object_id = OBJECT_ID('ProductDescriptions'))
                ALTER TABLE ProductDescriptions DROP CONSTRAINT PK_ProductDescription;
            ");

            // Adiciona a nova coluna DescriptionID com valor padrão
            migrationBuilder.AddColumn<int>(
                name: "DescriptionID",
                table: "ProductDescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Define a nova chave primária
            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDescription",
                table: "ProductDescriptions",
                column: "DescriptionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a coluna DescriptionID
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDescription",
                table: "ProductDescriptions");

            migrationBuilder.DropColumn(
                name: "DescriptionID",
                table: "ProductDescriptions");
        }
    }
}