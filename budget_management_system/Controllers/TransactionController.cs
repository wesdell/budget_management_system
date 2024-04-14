using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace budget_management_system.Controllers
{
	public class TransactionController : Controller
	{
		private readonly ITransactionDBActions _transactionService;
		private readonly IUserDBActions _userService;
		private readonly IAccountDBActions _accountService;

		public TransactionController(ITransactionDBActions transaction, IUserDBActions user, IAccountDBActions account)
		{
			this._transactionService = transaction;
			this._userService = user;
			this._accountService = account;
		}

		public async Task<IActionResult> CreateTransaction()
		{
			CreateTransactionViewModel model = new CreateTransactionViewModel();
			model.AccountList = await this.GetAccounts(this._userService.GetUserId());
			return View(model);
		}

		public async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
		{
			IEnumerable<AccountModel> accounts = await this._accountService.GetAccounts(userId);
			return accounts.Select(account => new SelectListItem(account.Name, account.Id.ToString()));
		}
	}
}
