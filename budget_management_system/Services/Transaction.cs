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

		public async Task<TransactionModel> GetTransactionById(int id, int userId)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			return await connection.QueryFirstOrDefaultAsync<TransactionModel>(
				@"SELECT ""Transaction"".id AS Id, ""Transaction"".user_id AS UserId,
				""Transaction"".account_id AS AccountId, ""Transaction"".category_id AS CategoryId,
				""Transaction"".amount AS Amount, ""Transaction"".created_at AS CreatedAt,
				""Transaction"".description AS ""Description"", Category.transaction_type_id AS TransactionTypeId
				FROM ""Transaction""
				INNER JOIN Category ON Category.id = ""Transaction"".category_id
				WHERE ""Transaction"".id = @Id AND ""Transaction"".user_id = @UserId",
				new { Id = id, UserId = userId }
				);
		}

		public async Task UpdateTransaction(TransactionModel transaction, int previosAccountId, int previousAmount)
		{
			using SqlConnection connection = new SqlConnection(this._connectionString);
			await connection.ExecuteAsync(
				"UpdateTransaction",
				new
				{
					id = transaction.Id,
					created_at = transaction.CreatedAt,
					amount = transaction.Amount,
					previous_amount = previousAmount,
					account_id = transaction.AccountId,
					previous_account_id = previosAccountId,
					category_id = transaction.CategoryId,
					description = transaction.Description
				},
				commandType: System.Data.CommandType.StoredProcedure
				);
		}
	}
}
