using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_management_system.Models
{
	public class CreateTransactionViewModel : TransactionModel
	{
		public IEnumerable<SelectListItem> AccountList { get; set; }
		public IEnumerable<CategoryModel> CategoryList { get; set; }
	}
}
