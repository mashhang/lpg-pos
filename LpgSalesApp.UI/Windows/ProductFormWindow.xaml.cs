using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
using System.Windows;

namespace LpgSalesApp.UI.Windows;

/// <summary>
/// Interaction logic for ProductFormWindow.xaml
/// </summary>
public partial class ProductFormWindow : Window
{
    public ProductFormWindow()
    {
        InitializeComponent();
        var productService = App.Services.GetService(typeof(IProductService)) as IProductService;
        this.DataContext = new ProductFormViewModel(productService!, this.Close, null);
    }

    public ProductFormWindow(ProductDto? existingProduct)
    {
        InitializeComponent();
        var productService = App.Services.GetService(typeof(IProductService)) as IProductService;
        this.DataContext = new ProductFormViewModel(productService!, this.Close, existingProduct);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
