using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IUserDBActions _userService;
		private readonly ICategoryDBActions _categoryService;

		public CategoryController(IUserDBActions user, ICategoryDBActions category)
		{
			this._userService = user;
			this._categoryService = category;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<CategoryModel> categories = await this._categoryService.GetCategories(this._userService.GetUserId());
			return View(categories);
		}

		[HttpGet]
		public IActionResult CreateCategory()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateCategory(int id)
		{
			CategoryModel category = await this._categoryService.GetCategoryById(id, this._userService.GetUserId());
			if (category is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(category);
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmDeleteCategory(int id)
		{
			CategoryModel category = await this._categoryService.GetCategoryById(id, this._userService.GetUserId());
			if (category is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryModel category)
		{
			if (!ModelState.IsValid)
			{
				return View(category);
			}

			category.UserId = this._userService.GetUserId();
			await this._categoryService.CreateCategory(category);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory(CategoryModel newCategory)
		{
			if (!ModelState.IsValid)
			{
				return View(newCategory);
			}

			CategoryModel category = await this._categoryService.GetCategoryById(newCategory.Id, this._userService.GetUserId());
			if (category is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			newCategory.UserId = this._userService.GetUserId();
			await this._categoryService.UpdateCategory(newCategory);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			CategoryModel category = await this._categoryService.GetCategoryById(id, this._userService.GetUserId());
			if (category is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			await this._categoryService.DeleteCategory(id);
			return RedirectToAction("Index");
		}
	}
}
