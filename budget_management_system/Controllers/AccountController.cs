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

		public AccountController(IAccountTypeDBActions accountType, IUserDBActions user)
		{
			this._accountType = accountType;
			this._user = user;
		}

		[HttpGet]
		public async Task<ActionResult> CreateAccount()
		{
			IEnumerable<AccountTypeModel> accountTypes = await _accountType.GetAccountTypes(_user.GetUserId());
			CreateAccountViewModel model = new CreateAccountViewModel();
			model.AccountList = accountTypes.Select(account => new SelectListItem(account.Name, account.Id.ToString()));
			return View(model);
		}
	}
}
