using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class AccountTypeController : Controller
	{
		public IActionResult CreateAccountType()
		{
			return View();
		}
	}
}
