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
	}
}
