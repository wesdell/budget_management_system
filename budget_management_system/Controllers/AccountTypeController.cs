﻿using budget_management_system.Interfaces;
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
		public async Task<IActionResult> Index()
		{
			IEnumerable<AccountTypeModel> accountTypes = await _accountType.GetAccountTypes(1);
			return View(accountTypes);
		}

		[HttpGet]
		public IActionResult CreateAccountType()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> CheckAccountTypeAlreadyExists(string name)
		{
			bool accountTypeAlreadyExists = await _accountType.AccountAlreadyExists(name, 1);
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

			accountTypeData.UserId = 1;

			bool recordExists = await _accountType.AccountAlreadyExists(accountTypeData.Name, accountTypeData.UserId);

			if (recordExists)
			{
				ModelState.AddModelError(nameof(accountTypeData.Name), $"{accountTypeData.Name} account already exists.");
				return View(accountTypeData);
			}

			await this._accountType.CreateAccountType(accountTypeData);

			return RedirectToAction("Index");
		}
	}
}