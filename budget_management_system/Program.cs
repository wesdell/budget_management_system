using budget_management_system.Interfaces;
using budget_management_system.Services;

namespace budget_management_system
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddTransient<IAccountTypeDBActions, AccountType>();
			builder.Services.AddTransient<IUserDBActions, User>();
			builder.Services.AddTransient<IAccountDBActions, Account>();
			builder.Services.AddTransient<ICategoryDBActions, Category>();
			builder.Services.AddTransient<ITransactionDBActions, Transaction>();
			builder.Services.AddAutoMapper(typeof(Program));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
