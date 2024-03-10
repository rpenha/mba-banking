using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Application.Migrations
{
    /// <inheritdoc />
    public partial class BankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank_account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    bank_branch = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    account_number = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    account_type = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    balance_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    balance_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    current_limit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    current_limit_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    total_limit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_limit_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bank_account", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bank_account_bank_branch_account_number",
                table: "bank_account",
                columns: new[] { "bank_branch", "account_number" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_account");
        }
    }
}
