using budget_management_system.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class AccountTypeModel : IAccountType
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "{0} field is required.")]
		[StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "{0} field must contain {2} - {1} characters.")]
		[Display(Name = "Account type")]
		public string Name { get; set; }
		public int Order { get; set; }
		public int UserId { get; set; }
	}
}
