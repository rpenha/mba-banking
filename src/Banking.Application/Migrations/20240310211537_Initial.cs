using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
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
                    is_using_limit = table.Column<bool>(type: "boolean", nullable: true),
                    current_limit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    current_limit_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    total_limit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_limit_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    used_limit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    used_limit_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tax_id = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    name_first_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name_last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    direction = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    transaction_type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    value_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    value_currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounts_bank_branch_account_number",
                table: "accounts",
                columns: new[] { "bank_branch", "account_number" });

            migrationBuilder.CreateIndex(
                name: "ix_customers_tax_id",
                table: "customers",
                column: "tax_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_account_id_timestamp",
                table: "transactions",
                columns: new[] { "account_id", "timestamp" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "transactions");
        }
    }
}
