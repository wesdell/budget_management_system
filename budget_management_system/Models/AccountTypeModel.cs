using budget_management_system.Interfaces;
using budget_management_system.Validations;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class AccountTypeModel : IAccountType
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "{0} field is required.")]
		[IsFirstLetterUpper]
		public string Name { get; set; }
		public int Order { get; set; }
		public int UserId { get; set; }
	}
}
