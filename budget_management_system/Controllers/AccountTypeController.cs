using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class AccountTypeController : Controller
	{
		private readonly IAccountTypeDBActions _accountType;

		public AccountTypeController(IAccountTypeDBActions accountTypeData)
		{
			this._accountType = accountTypeData;
		}

		[HttpGet]
		public IActionResult CreateAccountType()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateAccountType(AccountTypeModel accountTypeData)
		{
			if (!ModelState.IsValid)
			{
				return View(accountTypeData);
			}

			accountTypeData.UserId = 1;

			bool recordExists = await _accountType.AccountAlreadyExists(accountTypeData.Name, accountTypeData.UserId);

			if (recordExists)
			{
				ModelState.AddModelError(nameof(accountTypeData.Name), $"{accountTypeData.Name} account already exists.");
				return View(accountTypeData);
			}

			await this._accountType.CreateAccountType(accountTypeData);

			return View();
		}
	}
}
