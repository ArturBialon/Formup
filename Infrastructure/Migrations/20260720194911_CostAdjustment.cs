using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CostAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Costs_WorkCaseItemId",
                table: "Costs");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkCaseItemId",
                table: "Costs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_WorkCaseItemId",
                table: "Costs",
                column: "WorkCaseItemId",
                unique: true,
                filter: "[WorkCaseItemId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Costs_WorkCaseItemId",
                table: "Costs");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkCaseItemId",
                table: "Costs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Costs_WorkCaseItemId",
                table: "Costs",
                column: "WorkCaseItemId");
        }
    }
}
