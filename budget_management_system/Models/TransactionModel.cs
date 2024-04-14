using budget_management_system.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class TransactionModel : ITransaction
	{
		public int Id { get; set; }
		[StringLength(maximumLength: 150)]
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Today;
		public int UserId { get; set; }
		public int AccountId { get; set; }
		[Range(1, maximum: int.MaxValue)]
		public int CategoryId { get; set; }
	}
}
