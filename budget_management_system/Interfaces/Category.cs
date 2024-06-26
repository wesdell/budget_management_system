﻿using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface ICategory
	{
		int Id { get; set; }
		string Name { get; set; }
		ETransactionType TransactionTypeId { get; set; }
		int UserId { get; set; }
	}

	public interface ICategoryDBActions
	{
		Task CreateCategory(CategoryModel category);
		Task<IEnumerable<CategoryModel>> GetCategories(int userId);
		Task<IEnumerable<CategoryModel>> GetCategories(int userId, ETransactionType transactionType);
		Task<CategoryModel> GetCategoryById(int id, int userId);
		Task UpdateCategory(CategoryModel category);
		Task DeleteCategory(int id);
	}
}
