using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EPareH60Store.Models
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int CategoryId { get; set; }

        [NotMapped]
        public int CategoryID { get => CategoryId; set => CategoryId = value; }

        [Required]
        [StringLength(60)]
        public string ProdCat { get; set; } = null!;

        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; }

        public static async Task AddCategoryAsync(H60assignmentDbEpContext context, ProductCategory category)
        {
            await context.ProductCategories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public static async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync(H60assignmentDbEpContext context)
        {
            return await context.ProductCategories
                .OrderBy(c => c.ProdCat)
                .ToListAsync();
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
}
