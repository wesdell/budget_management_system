using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class AccountType : IAccountTypeDBActions
	{
		private readonly string _connectionString;

		public AccountType(IConfiguration configuration)
		{
			this._connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateAccountType(AccountTypeModel accountType)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			int id = await connection.QuerySingleAsync<int>(
				"CreateAccountType",
				new { user_id = accountType.UserId, name = accountType.Name },
				commandType: System.Data.CommandType.StoredProcedure
				);

			accountType.Id = id;
		}

		public async Task<bool> AccountAlreadyExists(string accountName, int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			int recordExists = await connection.QueryFirstOrDefaultAsync<int>(
				@"SELECT 1 FROM AccountType WHERE name = @Name AND user_id = @UserId;",
				new { Name = accountName, UserId = userId }
				);
			return recordExists == 1;
		}

		public async Task<IEnumerable<AccountTypeModel>> GetAccountTypes(int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryAsync<AccountTypeModel>(
				@"SELECT id, name, ""order"" FROM AccountType WHERE user_id = @UserId ORDER BY ""order""",
				new { UserId = userId }
				);
		}

		public async Task<AccountTypeModel> GetAccountTypeById(int id, int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryFirstOrDefaultAsync<AccountTypeModel>(
				@"SELECT id, name, ""order"" FROM AccountType WHERE id = @Id AND user_id = @UserId",
				new { Id = id, UserId = userId }
				);
		}

		public async Task UpdateAccountType(AccountTypeModel accountType)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			await connection.ExecuteAsync(
				@"UPDATE AccountType SET name = @Name WHERE id = @Id",
				accountType
				);
		}

		public async Task DeleteAccountType(int id)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			await connection.ExecuteAsync(
				@"DELETE AccountType WHERE id = @Id",
				new { Id = id }
				);
		}

		public async Task OrderAccountTypes(IEnumerable<AccountTypeModel> accountTypesOrdered)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			await connection.ExecuteAsync(
				@"UPDATE AccountType SET ""order"" = @Order WHERE id = @Id",
				accountTypesOrdered
				);
		}
	}
}
