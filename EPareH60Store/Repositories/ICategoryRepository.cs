using System.Collections.Generic;
using System.Threading.Tasks;
using EPareH60Store.Models;

namespace EPareH60Store.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<ProductCategory>> GetAllSortedAsync();
        Task<ProductCategory?> GetByIdAsync(int id);
        Task AddAsync(ProductCategory category);
        Task UpdateAsync(ProductCategory category);
        Task DeleteAsync(int id);
    }
}