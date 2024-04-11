using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_management_system.Models
{
	public class CreateAccountViewModel : AccountModel
	{
		public IEnumerable<SelectListItem> AccountList { get; set; }
	}
}
