using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Self_Suficient_Inventory_System.Migrations
{
    /// <inheritdoc />
    public partial class TestChangeTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillAudits");

            migrationBuilder.DropTable(
                name: "BillDetailAudits");

            migrationBuilder.DropTable(
                name: "OrderAudits");

            migrationBuilder.DropTable(
                name: "OrderDetailAudits");

            migrationBuilder.DropTable(
                name: "ProductAudits");

            migrationBuilder.DropTable(
                name: "SupplierAudits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemOperatorAudits",
                table: "SystemOperatorAudits");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "SystemOperatorAudits");

            migrationBuilder.DropColumn(
                name: "Pswd",
                table: "SystemOperatorAudits");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "SystemOperatorAudits");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "SystemOperatorAudits");

            migrationBuilder.RenameTable(
                name: "SystemOperatorAudits",
                newName: "Audits");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Audits",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "Audits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BillDetailAudit_Cantidad",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contacto",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetOcId",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHora",
                table: "Audits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSolicitud",
                table: "Audits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Ganancia",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdFactura",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdOc",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdOp",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProd",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProducto",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdProv",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OcId",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderAudit_IdOp",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdId",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvId",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMin",
                table: "Audits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SystemOperatorAudit_FechaBaja",
                table: "Audits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoAuditoria",
                table: "Audits",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Audits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Audits",
                table: "Audits",
                column: "AuditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Audits",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "BillDetailAudit_Cantidad",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Contacto",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "DetOcId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "FechaHora",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "FechaSolicitud",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Ganancia",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdFactura",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdOc",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdOp",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdProd",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "IdProv",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "OcId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "OrderAudit_IdOp",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ProdId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ProvId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "StockMin",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "SystemOperatorAudit_FechaBaja",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "TipoAuditoria",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Audits");

            migrationBuilder.RenameTable(
                name: "Audits",
                newName: "SystemOperatorAudits");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "AuditAction",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pswd",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Tipo",
                table: "SystemOperatorAudits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "SystemOperatorAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemOperatorAudits",
                table: "SystemOperatorAudits",
                column: "AuditId");

            migrationBuilder.CreateTable(
                name: "BillAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "BillDetailAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IdFactura = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetailAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "OrderAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProv = table.Column<int>(type: "int", nullable: false),
                    OcId = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    DetOcId = table.Column<int>(type: "int", nullable: false),
                    IdOc = table.Column<int>(type: "int", nullable: false),
                    IdProd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "ProductAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ganancia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProdId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    StockMin = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAudits", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "SupplierAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvId = table.Column<int>(type: "int", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierAudits", x => x.AuditId);
                });
        }
    }
}
