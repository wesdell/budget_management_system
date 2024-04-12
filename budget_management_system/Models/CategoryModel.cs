using budget_management_system.Interfaces;
using budget_management_system.Validations;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class CategoryModel : ICategory
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Category {0} is required.")]
		[StringLength(maximumLength: 50)]
		[IsFirstLetterUpper]
		public string Name { get; set; }
		public TransactionType TransactionTypeId { get; set; }
		public int UserId { get; set; }
	}
}
