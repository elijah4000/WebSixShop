using System;
using System.Globalization;
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
        public async Task<IActionResult> UpdateStock(int id, int stockChange = 1)
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

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepo.GetAllSortedAsync();
            ViewBag.ProdCatId = new SelectList(categories, "CategoryId", "ProdCat");
            return View(); // noop
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepo.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepo.GetAllSortedAsync();
            ViewBag.ProdCatId = new SelectList(categories, "CategoryId", "ProdCat", product?.ProdCatId);
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
                // Parse prices explicitly so non-numeric input causes an ArithmeticException as required
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

      
    }
}