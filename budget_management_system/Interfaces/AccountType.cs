namespace budget_management_system.Interfaces
{
	public interface IAccountType
	{
		int Id { get; set; }
		string Name { get; set; }
		int Order { get; set; }
		int UserId { get; set; }
	}
}
