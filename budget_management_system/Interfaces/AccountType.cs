using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface IAccountType
	{
		int Id { get; set; }
		string Name { get; set; }
		int Order { get; set; }
		int UserId { get; set; }
	}

	public interface IAccountTypeDBActions
	{
		Task CreateAccountType(AccountTypeModel accountType);
		Task<bool> AccountAlreadyExists(string accountName, int userId);
		Task<IEnumerable<AccountTypeModel>> GetAccountTypes(int userId);
		Task UpdateAccountType(AccountTypeModel accountType);
		Task<AccountTypeModel> GetAccountTypeById(int id, int userId);
		Task DeleteAccountType(int id);

	}
}
