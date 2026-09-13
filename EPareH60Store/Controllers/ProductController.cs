using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPareH60Store.Models;
using EPareH60Store.Repositories;

namespace EPareH60Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductsController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                var categoryProducts = await _productRepo.GetByCategorySortedAsync(categoryId.Value);
                return View(categoryProducts);
            }

            var allProducts = await _productRepo.GetAllSortedAsync();
            return View(allProducts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> UpdateStock(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int id, int stockChange)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            try
            {
                product.UpdateStock(stockChange);
                await _productRepo.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(product);
            }
        }

        public async Task<IActionResult> UpdatePrices(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrices(int id, decimal buyPrice, decimal sellPrice)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            try
            {
                product.UpdatePrices(buyPrice, sellPrice);
                await _productRepo.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(product);
            }
        }

      
    }
}