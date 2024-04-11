using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_management_system.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountTypeDBActions _accountType;
		private readonly IUserDBActions _user;
		private readonly IAccountDBActions _account;

		public AccountController(IAccountTypeDBActions accountType, IUserDBActions user, IAccountDBActions account)
		{
			this._accountType = accountType;
			this._user = user;
			this._account = account;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountModel> accounts = await this._account.GetAccounts(this._user.GetUserId());
			List<AccountIndexViewModel> model = accounts.GroupBy(account => account.AccountType).Select(group => new AccountIndexViewModel { AccountType = group.Key, Accounts = group.AsEnumerable() }).ToList();
			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> CreateAccount()
		{
			CreateAccountViewModel model = new CreateAccountViewModel();
			model.AccountList = await this.GetAccountTypes(_user.GetUserId());
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAccount(CreateAccountViewModel account)
		{
			AccountTypeModel accountType = await _accountType.GetAccountTypeById(account.AccountTypeId, _user.GetUserId());

			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			if (!ModelState.IsValid)
			{
				account.AccountList = await this.GetAccountTypes(_user.GetUserId());
				return View(account);
			}

			await _account.CreateAccount(account);
			return RedirectToAction("Index");
		}

		private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
		{
			IEnumerable<AccountTypeModel> accountTypes = await _accountType.GetAccountTypes(userId);
			return accountTypes.Select(account => new SelectListItem(account.Name, account.Id.ToString()));
		}
	}
}
