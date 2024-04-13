using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class Category : ICategoryDBActions
	{
		private readonly string _connectionString;

		public Category(IConfiguration configuration)
		{
			this._connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateCategory(CategoryModel category)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			int id = await connection.QuerySingleAsync<int>(
				@"INSERT INTO Category (name, transaction_type_id, user_id) VALUES (@Name, @TransactionTypeId, @UserId); SELECT SCOPE_IDENTITY()",
				category
				);
			category.Id = id;
		}

		public async Task<IEnumerable<CategoryModel>> GetCategories(int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryAsync<CategoryModel>(
				@"SELECT id, name, transaction_type_id AS TransactionTypeId, user_id AS UserId FROM Category WHERE user_id = @UserId",
				new { UserId = userId }
				);
		}

		public async Task<CategoryModel> GetCategoryById(int id, int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryFirstOrDefaultAsync<CategoryModel>(
				@"SELECT id, name, transaction_type_id AS TransactionTypeId, user_id AS UserId FROM Category WHERE id = @Id AND user_id = @UserId",
				new { Id = id, UserId = userId }
				);
		}

		public async Task UpdateCategory(CategoryModel category)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			await connection.ExecuteAsync(
				@"UPDATE Category SET name = @Name, transaction_type_id = @TransactionTypeId WHERE id = @Id",
				category
				);
		}
	}
}
