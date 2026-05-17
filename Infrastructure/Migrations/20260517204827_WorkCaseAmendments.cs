using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkCaseAmendments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForwardersId",
                table: "WorkCases",
                newName: "ForwarderId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkCases_ForwardersId",
                table: "WorkCases",
                newName: "IX_WorkCases_ForwarderId");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "WorkCases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WorkCases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAbandoned",
                table: "WorkCases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkCases_ClientId",
                table: "WorkCases",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "WorkCases_Clients",
                table: "WorkCases",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "WorkCases_Clients",
                table: "WorkCases");

            migrationBuilder.DropIndex(
                name: "IX_WorkCases_ClientId",
                table: "WorkCases");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "WorkCases");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WorkCases");

            migrationBuilder.DropColumn(
                name: "IsAbandoned",
                table: "WorkCases");

            migrationBuilder.RenameColumn(
                name: "ForwarderId",
                table: "WorkCases",
                newName: "ForwardersId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkCases_ForwarderId",
                table: "WorkCases",
                newName: "IX_WorkCases_ForwardersId");
        }
    }
}
