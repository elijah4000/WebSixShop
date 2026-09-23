using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPareH60Store.Migrations
{
    public partial class InitialAssignmentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreditCard = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCart_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "[Order]",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "date", nullable: false),
                    DateFulfilled = table.Column<DateTime>(type: "date", nullable: true),
                    Total = table.Column<decimal>(type: "numeric(10, 2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "numeric(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_ShoppingCart_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCart",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "[Order]",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            // seed customers
            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerId", "FirstName", "LastName", "Email", "PhoneNumber", "Province", "CreditCard" },
                values: new object[,]
                {
                    { 1, "Alice", "Smith", "alice@example.com", "1234567890", "ON", null },
                    { 2, "Bob", "Jones", "bob@example.com", "2345678901", "QC", null },
                    { 3, "Carol", "Brown", "carol@example.com", "3456789012", "BC", null }
                });

            // seed shopping cart
            migrationBuilder.InsertData(
                table: "ShoppingCart",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[] { 1, 2, DateTime.UtcNow.Date });

            // seed order
            migrationBuilder.InsertData(
                table: "[Order]",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Total", "Taxes" },
                values: new object[] { 1, 1, DateTime.UtcNow.Date.AddDays(-10), DateTime.UtcNow.Date.AddDays(-5), 59.97m, 5.00m });

            // seed cart items (must reference existing product ids)
            migrationBuilder.InsertData(
                table: "CartItem",
                columns: new[] { "CartItemId", "CartId", "ProductId", "Quantity", "Price" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 9.99m },
                    { 2, 1, 2, 1, 19.99m }
                });

            // seed order items
            migrationBuilder.InsertData(
                table: "OrderItem",
                columns: new[] { "OrderItemId", "OrderId", "ProductId", "Quantity", "Price" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 9.99m },
                    { 2, 1, 2, 1, 19.99m },
                    { 3, 1, 3, 1, 29.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_CustomerId",
                table: "ShoppingCart",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductId",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "[Order]",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("OrderItem", "OrderItemId", 1);
            migrationBuilder.DeleteData("OrderItem", "OrderItemId", 2);
            migrationBuilder.DeleteData("OrderItem", "OrderItemId", 3);

            migrationBuilder.DeleteData("CartItem", "CartItemId", 1);
            migrationBuilder.DeleteData("CartItem", "CartItemId", 2);

            migrationBuilder.DeleteData("[Order]", "OrderId", 1);

            migrationBuilder.DeleteData("ShoppingCart", "CartId", 1);

            migrationBuilder.DeleteData("Customer", "CustomerId", 1);
            migrationBuilder.DeleteData("Customer", "CustomerId", 2);
            migrationBuilder.DeleteData("Customer", "CustomerId", 3);

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "[Order]");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
