using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkCaseItemAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tax",
                table: "WorkCaseItems",
                newName: "TaxInvoice");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "WorkCaseItems",
                newName: "CurrencyCodeInvoice");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "WorkCaseItems",
                newName: "CostAmountNet");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountToInvoice",
                table: "WorkCaseItems",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCodeCost",
                table: "WorkCaseItems",
                type: "varchar(3)",
                unicode: false,
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountToInvoice",
                table: "WorkCaseItems");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeCost",
                table: "WorkCaseItems");

            migrationBuilder.RenameColumn(
                name: "TaxInvoice",
                table: "WorkCaseItems",
                newName: "Tax");

            migrationBuilder.RenameColumn(
                name: "CurrencyCodeInvoice",
                table: "WorkCaseItems",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CostAmountNet",
                table: "WorkCaseItems",
                newName: "Amount");
        }
    }
}
