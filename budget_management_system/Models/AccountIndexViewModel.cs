namespace budget_management_system.Models
{
	public class AccountIndexViewModel
	{
		public string AccountType { get; set; }
		public IEnumerable<AccountModel> Accounts { get; set; }
		public decimal TotalBalance => Accounts.Sum(x => x.Balance);

	}
}