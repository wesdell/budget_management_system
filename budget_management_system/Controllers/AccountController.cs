using AutoMapper;
using budget_management_system.Interfaces;
using budget_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_management_system.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountDBActions _accountService;
		private readonly ITransactionDBActions _transactionService;
		private readonly IMapper _mapper;
		private readonly IAccountTypeDBActions _accountTypeService;
		private readonly IUserDBActions _userService;

		public AccountController(IAccountTypeDBActions accountType, IUserDBActions user, IAccountDBActions account, ITransactionDBActions transaction, IMapper mapper)
		{
			this._accountTypeService = accountType;
			this._userService = user;
			this._accountService = account;
			this._transactionService = transaction;
			this._mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountModel> accounts = await this._accountService.GetAccounts(this._userService.GetUserId());
			List<AccountIndexViewModel> model = accounts.GroupBy(account => account.AccountType).Select(group => new AccountIndexViewModel { AccountType = group.Key, Accounts = group.AsEnumerable() }).ToList();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Report(int id, int month, int year)
		{
			AccountModel account = await this._accountService.GetAccountById(id, this._userService.GetUserId());
			if (account is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			DateTime CreatedAt;
			DateTime FinishedAt;

			if (month <= 0 || month > 12 || year <= 1900)
			{
				CreatedAt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			}
			else
			{
				CreatedAt = new DateTime(year, month, 1);
			}

			FinishedAt = CreatedAt.AddMonths(1).AddDays(-1);

			GetTransactionsByAccountModel transactionsByAccount = new GetTransactionsByAccountModel
			{
				AccountId = id,
				UserId = this._userService.GetUserId(),
				CreatedAt = CreatedAt,
				FinishedAt = FinishedAt
			};

			IEnumerable<TransactionModel> transactions = await this._transactionService.GetTransactionByAccount(transactionsByAccount);

			ReportTransactionModel model = new ReportTransactionModel();
			ViewBag.Account = account.Name;

			IEnumerable<ReportTransactionModel.TransactionsByDate> transactionsByDate = transactions
				.OrderByDescending(item => item.CreatedAt)
				.GroupBy(item => item.CreatedAt)
				.Select(transaction => new ReportTransactionModel.TransactionsByDate()
				{
					TransactionDate = transaction.Key,
					Transactions = transaction.AsEnumerable()
				});

			model.GroupedTransactions = transactionsByDate;
			model.CreatedAt = CreatedAt;
			model.FinishedAt = FinishedAt;
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

			CreateAccountViewModel model = this._mapper.Map<CreateAccountViewModel>(account);

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
