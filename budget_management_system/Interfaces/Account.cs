namespace budget_management_system.Interfaces
{
	public interface IAccount
	{
		int Id { get; set; }
		string Name { get; set; }
		decimal Balance { get; set; }
		string Description { get; set; }
		int AccountTypeId { get; set; }
	}
}
