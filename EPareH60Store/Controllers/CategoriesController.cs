using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EPareH60Store.Models;
using EPareH60Store.Repositories;

namespace EPareH60Store.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ICategoryRepository _categoryRepo;

		public CategoriesController(ICategoryRepository categoryRepo)
		{
			_categoryRepo = categoryRepo;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _categoryRepo.GetAllSortedAsync();
			return View(categories);
		}

		public async Task<IActionResult> Details(int id)
		{
			var category = await _categoryRepo.GetByIdAsync(id);
			if (category == null) return NotFound();
			return View(category);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryID,ProdCat")] ProductCategory category)
		{
			if (ModelState.IsValid)
			{
				await _categoryRepo.AddAsync(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var category = await _categoryRepo.GetByIdAsync(id);
			if (category == null) return NotFound();
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryID,ProdCat")] ProductCategory category)
		{
			if (id != category.CategoryID) return NotFound();

			if (ModelState.IsValid)
			{
				await _categoryRepo.UpdateAsync(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var category = await _categoryRepo.GetByIdAsync(id);
			if (category == null) return NotFound();
			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _categoryRepo.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
}