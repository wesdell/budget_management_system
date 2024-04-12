using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_management_system.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountDBActions _accountService;
		private readonly IAccountTypeDBActions _accountTypeService;
		private readonly IUserDBActions _userService;

		public AccountController(IAccountTypeDBActions accountType, IUserDBActions user, IAccountDBActions account)
		{
			this._accountTypeService = accountType;
			this._userService = user;
			this._accountService = account;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountModel> accounts = await this._accountService.GetAccounts(this._userService.GetUserId());
			List<AccountIndexViewModel> model = accounts.GroupBy(account => account.AccountType).Select(group => new AccountIndexViewModel { AccountType = group.Key, Accounts = group.AsEnumerable() }).ToList();
			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> CreateAccount()
		{
			CreateAccountViewModel model = new CreateAccountViewModel();
			model.AccountList = await this.GetAccountTypes(this._userService.GetUserId());
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateAccount(int id)
		{
			AccountModel account = await this._accountService.GetAccountById(id, this._userService.GetUserId());
			if (account == null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			CreateAccountViewModel model = new CreateAccountViewModel()
			{
				Id = account.Id,
				Name = account.Name,
				AccountTypeId = account.AccountTypeId,
				Balance = account.Balance,
				Description = account.Description
			};

			model.AccountList = await this.GetAccountTypes(this._userService.GetUserId());
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmDeleteAccount(int id)
		{
			AccountModel account = await this._accountService.GetAccountById(id, this._userService.GetUserId());
			if (account is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(account);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAccount(CreateAccountViewModel account)
		{
			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(account.AccountTypeId, this._userService.GetUserId());

			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			if (!ModelState.IsValid)
			{
				account.AccountList = await this.GetAccountTypes(this._userService.GetUserId());
				return View(account);
			}

			await this._accountService.CreateAccount(account);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccount(CreateAccountViewModel newAccount)
		{
			AccountModel account = await this._accountService.GetAccountById(newAccount.Id, this._userService.GetUserId());
			if (account is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(newAccount.AccountTypeId, this._userService.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			await this._accountService.UpdateAccount(newAccount);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			AccountModel account = await this._accountService.GetAccountById(id, this._userService.GetUserId());
			if (account is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			await this._accountService.DeleteAccount(id);
			return RedirectToAction("Index");
		}

		private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
		{
			IEnumerable<AccountTypeModel> accountTypes = await this._accountTypeService.GetAccountTypes(userId);
			return accountTypes.Select(account => new SelectListItem(account.Name, account.Id.ToString()));
		}
	}
}
