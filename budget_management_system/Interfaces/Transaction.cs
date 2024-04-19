using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface ITransaction
	{
		int Id { get; set; }
		int UserId { get; set; }
		int AccountId { get; set; }
		int CategoryId { get; set; }
		string Description { get; set; }
		string Account { get; set; }
		string Category { get; set; }
		decimal Amount { get; set; }
		DateTime CreatedAt { get; set; }
		ETransactionType TransactionTypeId { get; set; }
	}

	public interface ITransactionDBActions
	{
		Task CreateTransaction(TransactionModel transaction);
		Task<TransactionModel> GetTransactionById(int id, int userId);
		Task<IEnumerable<TransactionModel>> GetTransactionByAccount(GetTransactionsByAccountModel transactionByAccount);
		Task UpdateTransaction(TransactionModel transaction, int previosAccountId, decimal previousAmount);
		Task DeleteTransaction(int id);
	}

	public interface ITransactionAccount
	{
		int UserId { get; set; }
		int AccountId { get; set; }
		DateTime CreatedAt { get; set; }
		DateTime FinishedAt { get; set; }
	}
}
