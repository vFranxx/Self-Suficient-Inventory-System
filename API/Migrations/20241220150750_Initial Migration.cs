using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProdId = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProdId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    ProvId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.ProvId);
                });

            migrationBuilder.CreateTable(
                name: "SystemOperators",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<bool>(type: "bit", nullable: false),
                    Pswd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOperators", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProducts",
                columns: table => new
                {
                    IdProv = table.Column<int>(type: "int", nullable: false),
                    IdProd = table.Column<string>(type: "nvarchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProducts", x => new { x.IdProv, x.IdProd });
                    table.ForeignKey(
                        name: "FK_SupplierProducts_Products_IdProd",
                        column: x => x.IdProd,
                        principalTable: "Products",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierProducts_Suppliers_IdProv",
                        column: x => x.IdProv,
                        principalTable: "Suppliers",
                        principalColumn: "ProvId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    FacId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdOp = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.FacId);
                    table.ForeignKey(
                        name: "FK_Bills_SystemOperators_IdOp",
                        column: x => x.IdOp,
                        principalTable: "SystemOperators",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OcId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdOp = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IdProv = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OcId);
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_IdProv",
                        column: x => x.IdProv,
                        principalTable: "Suppliers",
                        principalColumn: "ProvId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_SystemOperators_IdOp",
                        column: x => x.IdOp,
                        principalTable: "SystemOperators",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    FacDetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdFactura = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<string>(type: "nvarchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.FacDetId);
                    table.ForeignKey(
                        name: "FK_BillDetails_Bills_IdFactura",
                        column: x => x.IdFactura,
                        principalTable: "Bills",
                        principalColumn: "FacId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_Products_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Products",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    DetOcId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IdProd = table.Column<string>(type: "nvarchar(13)", nullable: false),
                    IdOc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.DetOcId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_IdOc",
                        column: x => x.IdOc,
                        principalTable: "Orders",
                        principalColumn: "OcId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_IdProd",
                        column: x => x.IdProd,
                        principalTable: "Products",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_IdFactura",
                table: "BillDetails",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_IdProducto",
                table: "BillDetails",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_IdOp",
                table: "Bills",
                column: "IdOp");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IdOc",
                table: "OrderDetails",
                column: "IdOc");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IdProd",
                table: "OrderDetails",
                column: "IdProd");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdOp",
                table: "Orders",
                column: "IdOp");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdProv",
                table: "Orders",
                column: "IdProv");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_IdProd",
                table: "SupplierProducts",
                column: "IdProd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "SupplierProducts");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "SystemOperators");
        }
    }
}