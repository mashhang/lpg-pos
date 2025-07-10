using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LpgSalesApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTrackingToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_ModifiedByUserId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_ModifiedByUserId",
                table: "Transactions",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_ModifiedByUserId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_ModifiedByUserId",
                table: "Transactions",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
