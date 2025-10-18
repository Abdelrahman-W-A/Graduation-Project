using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingPercentagetoSalesItemandPurchasesItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPercentageAdded",
                table: "SalesInvoiceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "SalesInvoiceItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPercentageAdded",
                table: "PurchaseInvoiceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "PurchaseInvoiceItems",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPercentageAdded",
                table: "SalesInvoiceItems");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "SalesInvoiceItems");

            migrationBuilder.DropColumn(
                name: "IsPercentageAdded",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "PurchaseInvoiceItems");
        }
    }
}
