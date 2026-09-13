using System;
using Microsoft.EntityFrameworkCore;
using EPareH60Store.Models;

namespace EPareH60Store.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProdCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategory");
            });

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.ShoppingCart)
                .WithOne(s => s.Customer)
                .HasForeignKey<ShoppingCart>(s => s.CustomerId);

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@test.com", PhoneNumber = "5551234567", Province = "QC" },
                new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@test.com", PhoneNumber = "5559876543", Province = "ON" },
                new Customer { CustomerId = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie@test.com", PhoneNumber = "5555555555", Province = "BC" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, CustomerId = 1, DateCreated = new DateTime(2026, 9, 1), DateFulfilled = new DateTime(2026, 9, 5), Total = 150.00m, Taxes = 22.50m }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = 50.00m },
                new OrderItem { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 50.00m },
                new OrderItem { OrderItemId = 3, OrderId = 1, ProductId = 3, Quantity = 1, Price = 50.00m }
            );

            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart { CartId = 1, CustomerId = 2, DateCreated = new DateTime(2026, 9, 10) }
            );

            modelBuilder.Entity<CartItem>().HasData(
                new CartItem { CartItemId = 1, CartId = 1, ProductId = 1, Quantity = 2, Price = 50.00m },
                new CartItem { CartItemId = 2, CartId = 1, ProductId = 2, Quantity = 1, Price = 50.00m }
            );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}