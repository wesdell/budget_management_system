using budget_management_system.Interfaces;
using budget_management_system.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_management_system.Services
{
	public class Transaction : ITransactionDBActions
	{
		private readonly string _connectionString;

		public Transaction(IConfiguration configuration)
		{
			this._connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task CreateTransaction(TransactionModel transaction)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			int id = await connection.QuerySingleAsync<int>(
				"CreateTransaction",
				new
				{
					description = transaction.Description,
					amount = transaction.Amount,
					created_at = transaction.CreatedAt,
					user_id = transaction.UserId,
					account_id = transaction.AccountId,
					category_id = transaction.CategoryId
				},
				commandType: System.Data.CommandType.StoredProcedure
				);
			transaction.Id = id;
		}
	}
}
