using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingPDFFeatureTOInvoicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentContentType",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachmentData",
                table: "SalesInvoices",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFileName",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentContentType",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachmentData",
                table: "PurchaseInvoices",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFileName",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentContentType",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentData",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentFileName",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentContentType",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentData",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentFileName",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "PurchaseInvoices");
        }
    }
}
