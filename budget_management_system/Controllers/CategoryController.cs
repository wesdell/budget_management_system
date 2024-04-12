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
		public async Task<IActionResult> CreateCategory()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryModel category)
		{
			if (!ModelState.IsValid)
			{
				return View(category);
			}

			category.Id = this._userService.GetUserId();
			await this._categoryService.CreateCategory(category);
			return RedirectToAction("Index");
		}
	}
}
