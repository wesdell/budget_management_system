namespace budget_management_system.Models
{
	public class UpdateTransactionViewModel : CreateTransactionViewModel
	{
		public int PreviousAccountId { get; set; }
		public decimal PreviousAmount { get; set; }
		public string ReturnURL { get; set; }
	}
}
