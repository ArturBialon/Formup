using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkCaseCurrencyFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "WorkCases",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "WorkCases",
                newName: "CreatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "WorkCaseItems",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "WorkCaseItems",
                newName: "CreatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Invoices",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Costs",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Clients",
                newName: "CurrencyCode");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInPln",
                table: "WorkCases",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInPln",
                table: "WorkCases");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "WorkCases",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "WorkCases",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "WorkCaseItems",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "WorkCaseItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Invoices",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Costs",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Clients",
                newName: "Currency");
        }
    }
}
