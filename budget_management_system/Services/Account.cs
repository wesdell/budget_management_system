using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class Account : IAccountDBActions
	{
		private readonly string _connectionString;

		public Account(IConfiguration configuration)
		{
			this._connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateAccount(AccountModel account)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			int id = await connection.QuerySingleAsync<int>(
				@"INSERT INTO Account (name, description, balance, account_type_id) VALUES (@Name, @Description, @Balance, @AccountTypeId); SELECT SCOPE_IDENTITY()",
				account
				);

			account.Id = id;
		}

		public async Task<IEnumerable<AccountModel>> GetAccounts(int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryAsync<AccountModel>(
				@"SELECT acc.id, acc.name, acc.balance, acct.name AS account_type FROM Account AS acc
				INNER JOIN AccountType AS acct ON acc.account_type_id = acct.id
				WHERE acct.user_id = @UserId ORDER BY acct.""order""",
				new { UserId = userId }
				);
		}
	}
}