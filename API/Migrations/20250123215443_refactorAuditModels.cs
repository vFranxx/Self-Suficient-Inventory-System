using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class refactorAuditModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditAction",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuditAction",
                table: "SupplierAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuditAction",
                table: "ProductAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuditAction",
                table: "BillDetailAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuditAction",
                table: "BillAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditAction",
                table: "SystemOperatorAudits");

            migrationBuilder.DropColumn(
                name: "AuditAction",
                table: "SupplierAudits");

            migrationBuilder.DropColumn(
                name: "AuditAction",
                table: "ProductAudits");

            migrationBuilder.DropColumn(
                name: "AuditAction",
                table: "BillDetailAudits");

            migrationBuilder.DropColumn(
                name: "AuditAction",
                table: "BillAudits");
        }
    }
}