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

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //todo this can proabably be changed probably bad security
        => optionsBuilder.UseSqlServer("Server=tcp:csdevsql.cegep-heritage.qc.ca,1433; Database=H60AssignmentDB_EP;Authentication=Active Directory Interactive; Encrypt=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ProductID);

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(pc => pc.CategoryId);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProdCat)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
