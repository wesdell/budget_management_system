using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class AccountTypeController : Controller
	{
		[HttpGet]
		public IActionResult CreateAccountType()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateAccountType(AccountTypeModel accountType)
		{
			return View();
		}
	}
}
