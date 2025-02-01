using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTful_API.Migrations
{
    /// <inheritdoc />
    public partial class Probablythelastchangestothedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMin",
                table: "Products",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockMin",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    DepoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<string>(type: "nvarchar(13)", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMin = table.Column<int>(type: "int", nullable: false)
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
    }
}
