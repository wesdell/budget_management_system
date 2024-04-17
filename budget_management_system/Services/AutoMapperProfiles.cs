using AutoMapper;
using budget_management_system.Models;

namespace budget_management_system.Services
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AccountModel, CreateAccountViewModel>();
			CreateMap<UpdateTransactionViewModel, TransactionModel>().ReverseMap();
		}
	}
}
