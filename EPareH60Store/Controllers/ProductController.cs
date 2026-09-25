using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPareH60Store.Models;
using EPareH60Store.Repositories;

namespace EPareH60Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly Microsoft.Extensions.Logging.ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepo, ICategoryRepository categoryRepo, Microsoft.Extensions.Logging.ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        // Full product list: category, description, stock, sell price — sorted by category then product
        public async Task<IActionResult> Index()
        {
            var allProducts = await _productRepo.GetAllSortedAsync();
            return View(allProducts);
        }

        // Abbreviated list for one category: Description + SellPrice only
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryId);
            if (category == null) return NotFound();

            ViewData["CategoryName"] = category.ProdCat;
            ViewData["CategoryId"] = categoryId;

            var products = await _productRepo.GetByCategorySortedAsync(categoryId);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepo.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            LogModelStateErrors();
            await PopulateCategoriesDropDown(product.ProdCatId);
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            await PopulateCategoriesDropDown(product.ProdCatId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProdCatId,Description,Manufacturer")] Product product)
        {
            if (id != product.ProductID) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _productRepo.GetByIdWithCategoryAsync(id);
                if (existing == null) return NotFound();

                existing.ProdCatId = product.ProdCatId;
                existing.Description = product.Description;
                existing.Manufacturer = product.Manufacturer;
                await _productRepo.UpdateAsync(existing);
                return RedirectToAction(nameof(Index));
            }

            LogModelStateErrors();
            await PopulateCategoriesDropDown(product.ProdCatId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateStock(int id)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int id, [BindRequired] int stockChange)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(stockChange), "Stock change amount is required.");
                return View(product);
            }

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
        public async Task<IActionResult> UpdatePrices(int id, string buyPrice, string sellPrice)
        {
            var product = await _productRepo.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            try
            {
                if (!decimal.TryParse(buyPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out var buy))
                    throw new ArithmeticException("Buy price is not a number.");
                if (!decimal.TryParse(sellPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out var sell))
                    throw new ArithmeticException("Sell price is not a number.");

                product.UpdatePrices(buy, sell);
                await _productRepo.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(product);
            }
        }

        private async Task PopulateCategoriesDropDown(int? selectedId = null)
        {
            var categories = await _categoryRepo.GetAllSortedAsync();
            if (categories == null || !System.Linq.Enumerable.Any(categories))
            {
                ViewBag.ProdCatId = new SelectList(new[] { new SelectListItem("(No categories)", "0") }, "Value", "Text");
                ModelState.AddModelError(string.Empty, "No categories found.");
                return;
            }

            ViewBag.ProdCatId = new SelectList(categories, "CategoryId", "ProdCat", selectedId);
        }

        private void LogModelStateErrors()
        {
            foreach (var kv in ModelState)
            {
                foreach (var err in kv.Value.Errors)
                {
                    _logger.LogWarning("ModelState error for {Key}: {Error}", kv.Key, err.ErrorMessage);
                }
            }
        }
    }
}
