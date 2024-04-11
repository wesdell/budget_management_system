using budget_management_system.Interfaces;
using budget_management_system.Validations;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class AccountModel : IAccount
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Account {0} is required.")]
		[StringLength(maximumLength: 50)]
		[IsFirstLetterUpper]
		public string Name { get; set; }
		[StringLength(maximumLength: 150)]
		public string Description { get; set; }
		public decimal Balance { get; set; }
		[Display(Name = "Account type")]
		public int AccountTypeId { get; set; }
	}
}