using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class Account : IAccountDBActions
	{
		private readonly string connectionString;

		public Account(IConfiguration configuration)
		{
			this.connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateAccount(AccountModel account)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			int id = await connection.QuerySingleAsync<int>(
				@"INSERT INTO Account (name, description, balance, account_type_id) VALUES (@Name, @Description, @Balance, @AccountTypeId); SELECT SCOPE_IDENTITY()",
				account
				);

			account.Id = id;
		}
	}
}