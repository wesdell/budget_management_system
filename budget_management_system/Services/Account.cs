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
				@"SELECT Account.id, Account.name, Account.balance, AccountType.name AS AccountType FROM Account
				INNER JOIN AccountType ON AccountType.id = Account.account_type_id
				WHERE AccountType.user_id = 1 ORDER BY AccountType.""order""",
				new { UserId = userId }
				);
		}
	}
}