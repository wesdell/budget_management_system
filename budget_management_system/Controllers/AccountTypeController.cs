using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class AccountTypeController : Controller
	{
		private readonly IAccountTypeDBActions _accountTypeService;
		private readonly IUserDBActions _userService;

		public AccountTypeController(IAccountTypeDBActions accountType, IUserDBActions user)
		{
			this._accountTypeService = accountType;
			this._userService = user;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountTypeModel> accountTypes = await this._accountTypeService.GetAccountTypes(this._userService.GetUserId());
			return View(accountTypes);
		}

		[HttpGet]
		public IActionResult CreateAccountType()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateAccountType(int id)
		{
			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(id, this._userService.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(accountType);
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmDeleteAccountType(int id)
		{
			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(id, this._userService.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(accountType);
		}

		[HttpGet]
		public async Task<IActionResult> CheckAccountTypeAlreadyExists(string name)
		{
			bool accountTypeAlreadyExists = await this._accountTypeService.AccountAlreadyExists(name, this._userService.GetUserId());
			if (accountTypeAlreadyExists)
			{
				return Json($"{name} account already exists.");
			}
			return Json(true);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAccountType(AccountTypeModel accountTypeData)
		{
			if (!ModelState.IsValid)
			{
				return View(accountTypeData);
			}

			accountTypeData.UserId = this._userService.GetUserId();

			bool recordExists = await this._accountTypeService.AccountAlreadyExists(accountTypeData.Name, accountTypeData.UserId);

			if (recordExists)
			{
				ModelState.AddModelError(nameof(accountTypeData.Name), $"{accountTypeData.Name} account already exists.");
				return View(accountTypeData);
			}

			await this._accountTypeService.CreateAccountType(accountTypeData);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccountType(AccountTypeModel accountTypeData)
		{
			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(accountTypeData.Id, this._userService.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			await this._accountTypeService.UpdateAccountType(accountTypeData);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAccountType(int id)
		{
			AccountTypeModel accountType = await this._accountTypeService.GetAccountTypeById(id, this._userService.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			await this._accountTypeService.DeleteAccountType(id);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> OrderAccountTypes([FromBody] int[] newAccountTypes)
		{
			IEnumerable<AccountTypeModel> accountTypes = await this._accountTypeService.GetAccountTypes(this._userService.GetUserId());
			IEnumerable<int> accountTypesId = accountTypes.Select(acc => acc.Id);

			List<int> intrusiveAccountTypes = newAccountTypes.Except(accountTypesId).ToList();
			if (intrusiveAccountTypes.Count > 0)
			{
				return Forbid();
			}

			IEnumerable<AccountTypeModel> accountTypesOrdered = newAccountTypes.Select((value, i) => new AccountTypeModel { Id = value, Order = i + 1 }).AsEnumerable();
			await this._accountTypeService.OrderAccountTypes(accountTypesOrdered);
			return Ok();
		}
	}
}
