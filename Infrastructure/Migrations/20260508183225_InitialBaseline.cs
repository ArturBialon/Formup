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
                    TAX = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    ZIP = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Coutry = table.Column<string>(type: "varchar(54)", unicode: false, maxLength: 54, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
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
                name: "ServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TAX = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    ZIP = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Coutry = table.Column<string>(type: "varchar(54)", unicode: false, maxLength: 54, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Relation = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    ForwardersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCases", x => x.Id);
                    table.ForeignKey(
                        name: "WorkCases_Forwarders",
                        column: x => x.ForwardersId,
                        principalTable: "Forwarders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    TAX = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    WorkCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "Costs_Service_Providers",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
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
                    TAX = table.Column<int>(type: "int", nullable: false),
                    Issue_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Service_Date = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
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
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Amonut = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TAX = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "Service_Invoices",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Costs_ServiceProviderId",
                table: "Costs",
                column: "ServiceProviderId");

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
                name: "IX_Services_InvoiceId",
                table: "Services",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCases_ForwardersId",
                table: "WorkCases",
                column: "ForwardersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ServiceProviders");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "WorkCases");

            migrationBuilder.DropTable(
                name: "Forwarders");
        }
    }
}
