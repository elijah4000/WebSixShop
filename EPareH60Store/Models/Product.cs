using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EPareH60Store.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string? Description { get; set; }

    public string? Manufacturer { get; set; }

    public int Stock { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }

    public virtual ProductCategory ProdCat { get; set; } = null!;



      
        public static async Task AddProductAsync(H60assignmentDbEpContext context, Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public static async Task<IEnumerable<Product>> GetAllProductsAsync(H60assignmentDbEpContext context)
    {
        return await context.Products.Include(p => p.ProdCatId).ToListAsync();
    }

    public static async Task<Product?> GetProductByIdAsync(H60assignmentDbEpContext context, int id)
    {
        return await context.Products.Include(p => p.ProdCatId)
                                     .FirstOrDefaultAsync(p => p.ProductId == id);
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
    

