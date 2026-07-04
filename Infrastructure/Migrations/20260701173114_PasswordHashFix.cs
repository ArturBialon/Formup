using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordHashFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PassSalt",
                table: "Users",
                type: "varbinary(128)",
                unicode: false,
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary",
                oldUnicode: false);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PassHash",
                table: "Users",
                type: "varbinary(64)",
                unicode: false,
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "ServiceContractors",
                type: "nvarchar(85)",
                maxLength: 85,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PassSalt",
                table: "Users",
                type: "varbinary",
                unicode: false,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(128)",
                oldUnicode: false,
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PassHash",
                table: "Users",
                type: "varbinary",
                unicode: false,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(64)",
                oldUnicode: false,
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "ServiceContractors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(85)",
                oldMaxLength: 85);
        }
    }
}
