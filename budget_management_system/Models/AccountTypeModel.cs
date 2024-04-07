using budget_management_system.Interfaces;

namespace budget_management_system.Models
{
	public class AccountTypeModel : IAccountType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
		public int UserId { get; set; }
	}
}
