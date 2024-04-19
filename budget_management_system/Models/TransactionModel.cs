using budget_management_system.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class TransactionModel : ITransaction
	{
		public int Id { get; set; }
		[StringLength(maximumLength: 150)]
		public string Description { get; set; }
		public string Account { get; set; }
		public string Category { get; set; }
		public decimal Amount { get; set; }
		[Display(Name = "Created at")]
		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; } = DateTime.Today;
		public int UserId { get; set; }
		[Display(Name = "Account")]
		public int AccountId { get; set; }
		[Range(1, maximum: int.MaxValue)]
		[Display(Name = "Category")]
		public int CategoryId { get; set; }
		[Display(Name = "Transaction type")]
		public ETransactionType TransactionTypeId { get; set; } = ETransactionType.Income;
	}
}
