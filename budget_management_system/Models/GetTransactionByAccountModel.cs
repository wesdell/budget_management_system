using budget_management_system.Interfaces;

namespace budget_management_system.Models
{
	public class GetTransactionsByAccountModel : ITransactionAccount
	{
		public int UserId { get; set; }
		public int AccountId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime FinishedAt { get; set; }
	}
}
