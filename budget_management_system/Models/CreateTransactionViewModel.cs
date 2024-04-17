using budget_management_system.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace budget_management_system.Models
{
	public class CreateTransactionViewModel : TransactionModel
	{
		public IEnumerable<SelectListItem> AccountList { get; set; }
		public IEnumerable<SelectListItem> CategoryList { get; set; }
	}
}
