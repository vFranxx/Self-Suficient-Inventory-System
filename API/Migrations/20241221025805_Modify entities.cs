using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Modifyentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "Products",
                type: "decimal(2,1)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Ganancia",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    DepoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMin = table.Column<int>(type: "int", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdProducto = table.Column<string>(type: "nvarchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.DepoId);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Products",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IdProducto",
                table: "Inventories",
                column: "IdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Ganancia",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    DepoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<string>(type: "nvarchar(13)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    Ganancia = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.DepoId);
                    table.ForeignKey(
                        name: "FK_Warehouses_Products_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Products",
                        principalColumn: "ProdId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_IdProducto",
                table: "Warehouses",
                column: "IdProducto");
        }
    }
}