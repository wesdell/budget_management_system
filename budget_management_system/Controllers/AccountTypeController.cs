using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_management_system.Controllers
{
	public class AccountTypeController : Controller
	{
		private readonly IAccountTypeDBActions _accountType;
		private readonly IUserDBActions _userData;

		public AccountTypeController(IAccountTypeDBActions accountTypeData, IUserDBActions userData)
		{
			this._accountType = accountTypeData;
			this._userData = userData;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountTypeModel> accountTypes = await _accountType.GetAccountTypes(_userData.GetUserId());
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
			AccountTypeModel accountType = await this._accountType.GetAccountTypeById(id, this._userData.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(accountType);
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmDeleteAccountType(int id)
		{
			AccountTypeModel accountType = await this._accountType.GetAccountTypeById(id, this._userData.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			return View(accountType);
		}

		[HttpGet]
		public async Task<IActionResult> CheckAccountTypeAlreadyExists(string name)
		{
			bool accountTypeAlreadyExists = await _accountType.AccountAlreadyExists(name, _userData.GetUserId());
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

			accountTypeData.UserId = _userData.GetUserId();

			bool recordExists = await _accountType.AccountAlreadyExists(accountTypeData.Name, accountTypeData.UserId);

			if (recordExists)
			{
				ModelState.AddModelError(nameof(accountTypeData.Name), $"{accountTypeData.Name} account already exists.");
				return View(accountTypeData);
			}

			await this._accountType.CreateAccountType(accountTypeData);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccountType(AccountTypeModel accountTypeData)
		{
			AccountTypeModel accountType = await this._accountType.GetAccountTypeById(accountTypeData.Id, this._userData.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			await this._accountType.UpdateAccountType(accountTypeData);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAccountType(int id)
		{
			AccountTypeModel accountType = await this._accountType.GetAccountTypeById(id, this._userData.GetUserId());
			if (accountType is null)
			{
				return RedirectToAction("NotFound", "Home");
			}
			await this._accountType.DeleteAccountType(id);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> OrderAccountTypes([FromBody] int[] newAccountTypes)
		{
			IEnumerable<AccountTypeModel> accountTypes = await this._accountType.GetAccountTypes(_userData.GetUserId());
			IEnumerable<int> accountTypesId = accountTypes.Select(acc => acc.Id);

			List<int> intrusiveAccountTypes = newAccountTypes.Except(accountTypesId).ToList();
			if (intrusiveAccountTypes.Count > 0)
			{
				return Forbid();
			}

			IEnumerable<AccountTypeModel> accountTypesOrdered = newAccountTypes.Select((value, i) => new AccountTypeModel { Id = value, Order = i + 1 }).AsEnumerable();
			await this._accountType.OrderAccountTypes(accountTypesOrdered);
			return Ok();
		}
	}
}
