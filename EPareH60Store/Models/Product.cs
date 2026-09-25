using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPareH60Store.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public int ProductId { get => ProductID; set => ProductID = value; }

    [NotMapped]
    public int Product_Id_Alias { get => ProductID; set => ProductID = value; }

    public int ProdCatId { get; set; }

    public string? Description { get; set; }

    public string? Manufacturer { get; set; }

    public int Stock { get; set; }

    public decimal? BuyPrice { get; set; }

    [Display(Name = "Sell Price")]
    public decimal? SellPrice { get; set; }

    [Display(Name = "Buy Price")]
    [NotMapped]
    public decimal? BuyPrice_DisplayAlias { get => BuyPrice; set => BuyPrice = value; }

    // Navigation property is loaded by EF; form posts only ProdCatId.
    // Without ValidateNever, nullable-enabled validation requires Category and blocks Create.
    [ValidateNever]
    public virtual ProductCategory Category { get; set; } = null!;

    public void UpdateStock(int stockChange)
    {
        var newStock = Stock + stockChange;
        if (newStock < 0) throw new System.InvalidOperationException("Stock cannot be negative.");
        Stock = newStock;
    }

    public void UpdatePrices(decimal buyPrice, decimal sellPrice)
    {
        if (buyPrice < 0 || sellPrice < 0) throw new System.ArgumentException("Prices cannot be negative.");

        buyPrice = System.Math.Round(buyPrice, 2);
        sellPrice = System.Math.Round(sellPrice, 2);

        if (sellPrice < buyPrice) throw new System.ArgumentException("Sell price cannot be less than buy price.");

        BuyPrice = buyPrice;
        SellPrice = sellPrice;
    }

    public static async Task AddProductAsync(H60assignmentDbEpContext context, Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public static async Task<IEnumerable<Product>> GetAllProductsAsync(H60assignmentDbEpContext context)
    {
        return await context.Products.Include(p => p.Category).ToListAsync();
    }

    public static async Task<Product?> GetProductByIdAsync(H60assignmentDbEpContext context, int id)
    {
        return await context.Products.Include(p => p.Category)
                                     .FirstOrDefaultAsync(p => p.ProductID == id);
    }

    public static async Task UpdateProductAsync(H60assignmentDbEpContext context, Product product)
    {
        context.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public static async Task DeleteProductAsync(H60assignmentDbEpContext context, int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
