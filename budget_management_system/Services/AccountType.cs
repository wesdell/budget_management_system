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

		public void CreateAccountType(AccountTypeModel accountType)
		{
			using SqlConnection connection = new SqlConnection(connectionString);
			int id = connection.QuerySingle<int>($@"INSERT INTO AccountType (name, user_id, ""order"") VALUES (@Name, @UserId, 0); SELECT SCOPE_IDENTITY();", accountType);

			accountType.Id = id;
		}
	}
}
