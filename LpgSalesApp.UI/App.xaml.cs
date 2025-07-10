using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.Infrastructure.Persistence;
using LpgSalesApp.Infrastructure.Services;
using LpgSalesApp.UI.Services;
using LpgSalesApp.UI.Views.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LpgSalesApp.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    public static UserDto? CurrentUser { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=lpg.db"));

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        Services = services.BuildServiceProvider();

        //SEED SAMPLE DATA
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.Seed(context);

        //var admin = context.Users.FirstOrDefault(u => u.Username == "admin");
        //if (admin == null)
        //{
        //    MessageBox.Show("Admin user is NOT in the database.");
        //}
        //else
        //{
        //    MessageBox.Show($"Admin found: {admin.Username}, Role: {admin.Role}");
        //}

        base.OnStartup(e);
        var login = new LoginWindow();
        login.Show();
    }

    //public App()
    //{
    //    var services = new ServiceCollection();
    //    services.AddSingleton<ITransactionService, TransactionService>();
    //    Services = services.BuildServiceProvider();
    //}

}

