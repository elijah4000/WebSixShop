using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EPareH60Store.Models;

public partial class H60assignmentDbEpContext : DbContext
{
    public H60assignmentDbEpContext()
    {
    }

    public H60assignmentDbEpContext(DbContextOptions<H60assignmentDbEpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; } // modified

    // Code-first DbSets for customer/order/cart functionality
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //todo this can proabably be changed probably bad security
        => optionsBuilder.UseSqlServer("Server=tcp:csdevsql.cegep-heritage.qc.ca,1433; Database=H60AssignmentDB_EP;Authentication=Active Directory Interactive; Encrypt=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasKey(e => e.ProductID);
            entity.Ignore(e => e.ProductId);
            entity.Ignore(e => e.Product_Id_Alias);
            entity.Ignore(e => e.BuyPrice_DisplayAlias);

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            // DB column is IDENTITY; seeded HasData IDs must not make EF send ProductID=0 on insert
            entity.Property(e => e.ProductID)
                .HasColumnName("ProductID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Description).HasMaxLength(80).IsUnicode(false);
            entity.Property(e => e.Manufacturer).HasMaxLength(80).IsUnicode(false);
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .HasPrincipalKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });
        // Seed product categories (at least 5) and products (at least 4 per category)
        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { CategoryId = 1, ProdCat = "Electronics" },
            new ProductCategory { CategoryId = 2, ProdCat = "Books" },
            new ProductCategory { CategoryId = 3, ProdCat = "Home" },
            new ProductCategory { CategoryId = 4, ProdCat = "Toys" },
            new ProductCategory { CategoryId = 5, ProdCat = "Clothing" }
        );

        modelBuilder.Entity<Product>().HasData(
            // Electronics (1..4)
            new Product { ProductID = 1, ProdCatId = 1, Description = "USB-C Charger", Manufacturer = "Acme", Stock = 10, BuyPrice = 5.00m, SellPrice = 9.99m },
            new Product { ProductID = 2, ProdCatId = 1, Description = "Wireless Mouse", Manufacturer = "Acme", Stock = 15, BuyPrice = 8.00m, SellPrice = 19.99m },
            new Product { ProductID = 3, ProdCatId = 1, Description = "Bluetooth Speaker", Manufacturer = "Acme", Stock = 8, BuyPrice = 12.00m, SellPrice = 29.99m },
            new Product { ProductID = 4, ProdCatId = 1, Description = "HDMI Cable", Manufacturer = "Acme", Stock = 25, BuyPrice = 2.00m, SellPrice = 6.99m },
            // Books (5..8)
            new Product { ProductID = 5, ProdCatId = 2, Description = "C# In Depth", Manufacturer = "TechPub", Stock = 12, BuyPrice = 20.00m, SellPrice = 39.99m },
            new Product { ProductID = 6, ProdCatId = 2, Description = "Learning EF Core", Manufacturer = "TechPub", Stock = 7, BuyPrice = 18.00m, SellPrice = 34.99m },
            new Product { ProductID = 7, ProdCatId = 2, Description = "ASP.NET Core Guide", Manufacturer = "TechPub", Stock = 9, BuyPrice = 17.50m, SellPrice = 29.99m },
            new Product { ProductID = 8, ProdCatId = 2, Description = "LINQ Cookbook", Manufacturer = "TechPub", Stock = 5, BuyPrice = 10.00m, SellPrice = 24.99m },
            // Home (9..12)
            new Product { ProductID = 9, ProdCatId = 3, Description = "Stainless Knife Set", Manufacturer = "HomeGoods", Stock = 6, BuyPrice = 25.00m, SellPrice = 49.99m },
            new Product { ProductID = 10, ProdCatId = 3, Description = "Non-stick Pan", Manufacturer = "HomeGoods", Stock = 14, BuyPrice = 15.00m, SellPrice = 29.99m },
            new Product { ProductID = 11, ProdCatId = 3, Description = "Vacuum Cleaner", Manufacturer = "HomeGoods", Stock = 4, BuyPrice = 80.00m, SellPrice = 149.99m },
            new Product { ProductID = 12, ProdCatId = 3, Description = "LED Bulb Pack", Manufacturer = "HomeGoods", Stock = 30, BuyPrice = 3.00m, SellPrice = 7.99m },
            // Toys (13..16)
            new Product { ProductID = 13, ProdCatId = 4, Description = "Building Blocks", Manufacturer = "PlayCo", Stock = 20, BuyPrice = 10.00m, SellPrice = 24.99m },
            new Product { ProductID = 14, ProdCatId = 4, Description = "Puzzle 1000pc", Manufacturer = "PlayCo", Stock = 11, BuyPrice = 5.00m, SellPrice = 12.99m },
            new Product { ProductID = 15, ProdCatId = 4, Description = "Remote Car", Manufacturer = "PlayCo", Stock = 3, BuyPrice = 18.00m, SellPrice = 39.99m },
            new Product { ProductID = 16, ProdCatId = 4, Description = "Doll", Manufacturer = "PlayCo", Stock = 9, BuyPrice = 7.00m, SellPrice = 19.99m },
            // Clothing (17..20)
            new Product { ProductID = 17, ProdCatId = 5, Description = "T-Shirt", Manufacturer = "ClothCo", Stock = 40, BuyPrice = 4.00m, SellPrice = 14.99m },
            new Product { ProductID = 18, ProdCatId = 5, Description = "Jeans", Manufacturer = "ClothCo", Stock = 18, BuyPrice = 12.00m, SellPrice = 39.99m },
            new Product { ProductID = 19, ProdCatId = 5, Description = "Jacket", Manufacturer = "ClothCo", Stock = 6, BuyPrice = 25.00m, SellPrice = 69.99m },
            new Product { ProductID = 20, ProdCatId = 5, Description = "Socks (3 pack)", Manufacturer = "ClothCo", Stock = 60, BuyPrice = 1.50m, SellPrice = 4.99m }
        );

        // Seed initial data for code-first tables (Customers, Orders, ShoppingCart, CartItem, OrderItem)
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", PhoneNumber = "1234567890", Province = "ON", CreditCard = null },
            new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", PhoneNumber = "2345678901", Province = "QC", CreditCard = null },
            new Customer { CustomerId = 3, FirstName = "Carol", LastName = "Brown", Email = "carol@example.com", PhoneNumber = "3456789012", Province = "BC", CreditCard = null }
        );

        // Shopping cart for customer 2 with two items
        modelBuilder.Entity<ShoppingCart>().HasData(
            new ShoppingCart { CartId = 1, CustomerId = 2, DateCreated = System.DateTime.UtcNow }
        );

        // Orders: customer 1 has a completed order
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, CustomerId = 1, DateCreated = System.DateTime.UtcNow.AddDays(-10), DateFulfilled = System.DateTime.UtcNow.AddDays(-5), Total = 0m, Taxes = 0m }
        );

        modelBuilder.Entity<CartItem>().HasData(
            new CartItem { CartItemId = 1, CartId = 1, ProductId = 1, Quantity = 2, Price = 9.99m },
            new CartItem { CartItemId = 2, CartId = 1, ProductId = 2, Quantity = 1, Price = 19.99m }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = 9.99m },
            new OrderItem { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 19.99m },
            new OrderItem { OrderItemId = 3, OrderId = 1, ProductId = 3, Quantity = 1, Price = 29.99m }
        );

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(pc => pc.CategoryId);
            entity.Ignore(pc => pc.CategoryID);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId)
                .HasColumnName("CategoryID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ProdCat).HasMaxLength(60).IsUnicode(false);

            entity.HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.ProdCatId)
                .HasPrincipalKey(pc => pc.CategoryId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
