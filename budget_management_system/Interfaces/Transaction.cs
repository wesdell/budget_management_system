using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface ITransaction
	{
		int Id { get; set; }
		string Description { get; set; }
		decimal Amount { get; set; }
		DateTime CreatedAt { get; set; }
		int UserId { get; set; }
		int AccountId { get; set; }
		int CategoryId { get; set; }
		ETransactionType TransactionTypeId { get; set; }
	}

	public interface ITransactionDBActions
	{
		Task CreateTransaction(TransactionModel transaction);
		Task<TransactionModel> GetTransactionById(int id, int userId);
		Task UpdateTransaction(TransactionModel transaction, int previosAccountId, int previousAmount);
	}
}
