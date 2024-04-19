using budget_management_system.Interfaces;

namespace budget_management_system.Models
{
	public class ReportTransactionModel : IReportTransaction
	{
		public DateTime CreatedAt { get; set; }
		public DateTime FinishedAt { get; set; }
		public IEnumerable<TransactionsByDate> GroupedTransactions { get; set; }
		public decimal IncomeAmount => GroupedTransactions.Sum(item => item.IncomeBalance);
		public decimal ExpenseAmount => GroupedTransactions.Sum(item => item.ExpenseBalance);
		public decimal Total => IncomeAmount - ExpenseAmount;
		public class TransactionsByDate
		{
			public DateTime TransactionDate { get; set; }
			public IEnumerable<TransactionModel> Transactions { get; set; }
			public decimal IncomeBalance => Transactions.Where(transaction => transaction.TransactionTypeId == ETransactionType.Income).Sum(item => item.Amount);
			public decimal ExpenseBalance => Transactions.Where(transaction => transaction.TransactionTypeId == ETransactionType.Expense).Sum(item => item.Amount);
		}
	}
}
