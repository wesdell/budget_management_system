﻿using budget_management_system.Interfaces;
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
		private readonly ICategoryDBActions _categoryService;

		public TransactionController(ITransactionDBActions transaction, IUserDBActions user, IAccountDBActions account, ICategoryDBActions category)
		{
			this._transactionService = transaction;
			this._userService = user;
			this._accountService = account;
			this._categoryService = category;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> CreateTransaction()
		{
			CreateTransactionViewModel model = new CreateTransactionViewModel();
			model.AccountList = await this.GetAccounts(this._userService.GetUserId());
			model.CategoryList = await this.GetCategories(this._userService.GetUserId(), model.TransactionTypeId);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTransaction(CreateTransactionViewModel transactionModel)
		{
			if (!ModelState.IsValid)
			{
				transactionModel.AccountList = await this.GetAccounts(this._userService.GetUserId());
				transactionModel.CategoryList = await this.GetCategories(this._userService.GetUserId(), transactionModel.TransactionTypeId);
				return View(transactionModel);
			}

			AccountModel account = await this._accountService.GetAccountById(transactionModel.AccountId, this._userService.GetUserId());
			if (account is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			CategoryModel category = await this._categoryService.GetCategoryById(transactionModel.CategoryId, this._userService.GetUserId());
			if (category is null)
			{
				return RedirectToAction("NotFound", "Home");
			}

			transactionModel.UserId = this._userService.GetUserId();
			if (transactionModel.TransactionTypeId == ETransactionType.Expense)
			{
				transactionModel.Amount *= -1;
			}

			await this._transactionService.CreateTransaction(transactionModel);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> GetCategories([FromBody] ETransactionType transactionType)
		{
			IEnumerable<SelectListItem> categories = await this.GetCategories(this._userService.GetUserId(), transactionType);
			return Ok(categories);
		}

		private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
		{
			IEnumerable<AccountModel> accounts = await this._accountService.GetAccounts(userId);
			return accounts.Select(account => new SelectListItem(account.Name, account.Id.ToString()));
		}

		private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, ETransactionType transactionType)
		{
			IEnumerable<CategoryModel> categories = await this._categoryService.GetCategories(userId, transactionType);
			return categories.Select(category => new SelectListItem(category.Name, category.Id.ToString()));
		}
	}
}
