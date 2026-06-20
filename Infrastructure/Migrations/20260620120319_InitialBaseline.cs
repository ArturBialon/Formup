using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    Zip = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Coutry = table.Column<string>(type: "varchar(54)", unicode: false, maxLength: 54, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forwarders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Prefix = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    PassHash = table.Column<byte[]>(type: "varbinary", unicode: false, nullable: false),
                    PassSalt = table.Column<byte[]>(type: "varbinary", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forwarders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceContractors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(54)", maxLength: 54, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Zip = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    ApartmentNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "varchar(254)", unicode: false, maxLength: 254, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceContractors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Relation = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsAbandoned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForwarderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCases", x => x.Id);
                    table.ForeignKey(
                        name: "WorkCases_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "WorkCases_Forwarders",
                        column: x => x.ForwarderId,
                        principalTable: "Forwarders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Tax = table.Column<int>(type: "int", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    WorkCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceContractorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "Costs_Service_Contractors",
                        column: x => x.ServiceContractorId,
                        principalTable: "ServiceContractors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Costs_WorkCases",
                        column: x => x.WorkCaseId,
                        principalTable: "WorkCases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax = table.Column<int>(type: "int", unicode: false, maxLength: 20, nullable: false),
                    Issue_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Service_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    WorkCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "Invoices_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Invoices_WorkCases",
                        column: x => x.WorkCaseId,
                        principalTable: "WorkCases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkCaseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsInvoiced = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkCaseItems_Invoices",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkCaseItems_WorkCases",
                        column: x => x.WorkCaseId,
                        principalTable: "WorkCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Costs_ServiceContractorId",
                table: "Costs",
                column: "ServiceContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_WorkCaseId",
                table: "Costs",
                column: "WorkCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_WorkCaseId",
                table: "Invoices",
                column: "WorkCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCaseItems_InvoiceId",
                table: "WorkCaseItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCaseItems_WorkCaseId",
                table: "WorkCaseItems",
                column: "WorkCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCases_ClientId",
                table: "WorkCases",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCases_ForwarderId",
                table: "WorkCases",
                column: "ForwarderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "WorkCaseItems");

            migrationBuilder.DropTable(
                name: "ServiceContractors");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "WorkCases");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Forwarders");
        }
    }
}
