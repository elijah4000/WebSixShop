using System.Collections.Generic;
using System.Threading.Tasks;
using EPareH60Store.Models;

namespace EPareH60Store.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllSortedAsync();
        Task<IEnumerable<Product>> GetByCategorySortedAsync(int categoryId);
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}