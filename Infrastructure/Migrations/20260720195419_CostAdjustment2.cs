using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CostAdjustment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Costs_WorkCases_WorkCaseId",
                table: "Costs");

            migrationBuilder.DropIndex(
                name: "IX_Costs_WorkCaseId",
                table: "Costs");

            migrationBuilder.DropColumn(
                name: "WorkCaseId",
                table: "Costs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkCaseId",
                table: "Costs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Costs_WorkCaseId",
                table: "Costs",
                column: "WorkCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Costs_WorkCases_WorkCaseId",
                table: "Costs",
                column: "WorkCaseId",
                principalTable: "WorkCases",
                principalColumn: "Id");
        }
    }
}
