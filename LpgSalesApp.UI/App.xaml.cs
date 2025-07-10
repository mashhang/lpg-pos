using System.Configuration;
using System.Data;
using System.Windows;
using LpgSalesApp.Infrastructure.Persistence;
using LpgSalesApp.Infrastructure.Services;
using LpgSalesApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LpgSalesApp.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=lpg.db"));

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ITransactionService, TransactionService>();

        Services = services.BuildServiceProvider();

        //SEED SAMPLE DATA
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.Seed(context);

        base.OnStartup(e);
    }

    public App()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITransactionService, TransactionService>();
        Services = services.BuildServiceProvider();
    }
}

