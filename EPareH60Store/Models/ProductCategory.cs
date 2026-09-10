using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EPareH60Store.Models;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public string ProdCat { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();




        public static async Task AddCategoryAsync(H60assignmentDbEpContext context, ProductCategory category)
        {
            await context.ProductCategories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public static async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync(H60assignmentDbEpContext context)
        {
            return await context.ProductCategories.ToListAsync();
        }

        public static async Task<ProductCategory?> GetCategoryByIdAsync(H60assignmentDbEpContext context, int id)
        {
            return await context.ProductCategories.FindAsync(id);
        }

        public static async Task UpdateCategoryAsync(H60assignmentDbEpContext context, ProductCategory category)
        {
            context.Entry(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        
        public static async Task DeleteCategoryAsync(H60assignmentDbEpContext context, int id)
        {
            var category = await context.ProductCategories.FindAsync(id);
            if (category != null)
            {
                context.ProductCategories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }


