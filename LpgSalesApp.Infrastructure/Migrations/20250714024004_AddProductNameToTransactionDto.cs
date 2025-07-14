using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LpgSalesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductNameToTransactionDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "TransactionItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "TransactionItems");
        }
    }
}
