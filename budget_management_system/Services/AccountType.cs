using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class AccountType : IAccountTypeDBActions
	{
		private readonly string connectionString;

		public AccountType(IConfiguration configuration)
		{
			this.connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateAccountType(AccountTypeModel accountType)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			int id = await connection.QuerySingleAsync<int>(
				@"INSERT INTO AccountType (name, user_id, ""order"") VALUES (@Name, @UserId, 0); SELECT SCOPE_IDENTITY();",
				accountType);

			accountType.Id = id;
		}

		public async Task<bool> AccountAlreadyExists(string accountName, int userId)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			int recordExists = await connection.QueryFirstOrDefaultAsync<int>(
				@"SELECT 1 FROM AccountType WHERE name = @Name AND user_id = @UserId;",
				new { Name = accountName, UserId = userId }
				);
			return recordExists == 1;
		}

		public async Task<IEnumerable<AccountTypeModel>> GetAccountTypes(int userId)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			return await connection.QueryAsync<AccountTypeModel>(
				@"SELECT id, name, ""order"" FROM AccountType WHERE user_id = @UserId",
				new { UserId = userId }
				);
		}

		public async Task<AccountTypeModel> GetAccountTypeById(int id, int userId)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			return await connection.QueryFirstOrDefaultAsync<AccountTypeModel>(
				@"SELECT id, name, ""order"" FROM AccountType WHERE id = @Id AND user_id = @UserId",
				new { Id = id, UserId = userId }
				);
		}

		public async Task UpdateAccountType(AccountTypeModel accountType)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync(
				@"UPDATE AccountType SET name = @Name WHERE id = @Id",
				accountType
				);
		}
	}
}
