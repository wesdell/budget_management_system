using budget_management_system.Models;

namespace budget_management_system.Interfaces
{
	public interface IAccount
	{
		int Id { get; set; }
		string Name { get; set; }
		decimal Balance { get; set; }
		string Description { get; set; }
		string AccountType { get; set; }
		int AccountTypeId { get; set; }
	}

	public interface IAccountDBActions
	{
		Task CreateAccount(AccountModel account);
		Task<IEnumerable<AccountModel>> GetAccounts(int userId);
		Task<AccountModel> GetAccountById(int id, int userId);
		Task UpdateAccount(CreateAccountViewModel account);
		Task DeleteAccount(int id);
	}
}
