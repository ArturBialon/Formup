using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Costs_Service_Contractors",
                table: "Costs");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "WorkCases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IBAN = table.Column<string>(type: "varchar(34)", unicode: false, maxLength: 34, nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false, defaultValue: "PLN"),
                    IsMain = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ServiceContractorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankAccounts_ServiceContractors",
                        column: x => x.ServiceContractorId,
                        principalTable: "ServiceContractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ClientId",
                table: "BankAccounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ServiceContractorId",
                table: "BankAccounts",
                column: "ServiceContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Costs_ServiceContractors",
                table: "Costs",
                column: "ServiceContractorId",
                principalTable: "ServiceContractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Costs_ServiceContractors",
                table: "Costs");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "WorkCases");

            migrationBuilder.AddForeignKey(
                name: "Costs_Service_Contractors",
                table: "Costs",
                column: "ServiceContractorId",
                principalTable: "ServiceContractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
