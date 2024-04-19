using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface IReportTransaction
	{
		DateTime CreatedAt { get; set; }
		DateTime FinishedAt { get; set; }
		decimal IncomeAmount { get; }
		decimal ExpenseAmount { get; }
		decimal Total { get; }
		IEnumerable<TransactionsByDate> GroupedTransactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		class TransactionsByDate
		{
			DateTime TransactionDate { get; set; }
			IEnumerable<TransactionModel> Transactions { get; set; }
			decimal IncomeBalance;
			decimal ExpenseBalance;
		}
	}
}
}
