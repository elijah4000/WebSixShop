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

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasOne(c => c.ShoppingCart)
                .WithOne(sc => sc.Customer)
                .HasForeignKey<ShoppingCart>(sc => sc.CustomerId);
        });

        modelBuilder.Entity<ShoppingCart>(entity => entity.ToTable("ShoppingCart"));
        modelBuilder.Entity<CartItem>(entity => entity.ToTable("CartItem"));
        modelBuilder.Entity<Order>(entity => entity.ToTable("Order"));
        modelBuilder.Entity<OrderItem>(entity => entity.ToTable("OrderItem"));

        // Part B seed only (Product/ProductCategory already exist from the SQL template / manual data)
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", PhoneNumber = "1234567890", Province = "ON", CreditCard = null },
            new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", PhoneNumber = "2345678901", Province = "QC", CreditCard = null },
            new Customer { CustomerId = 3, FirstName = "Carol", LastName = "Brown", Email = "carol@example.com", PhoneNumber = "3456789012", Province = "BC", CreditCard = null }
        );

        modelBuilder.Entity<ShoppingCart>().HasData(
            new ShoppingCart { CartId = 1, CustomerId = 2, DateCreated = new DateTime(2026, 1, 15) }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, CustomerId = 1, DateCreated = new DateTime(2026, 1, 5), DateFulfilled = new DateTime(2026, 1, 10), Total = 59.97m, Taxes = 5.00m }
        );

        // CartItem / OrderItem seeded in the migration SQL using whatever ProductIDs already exist

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
