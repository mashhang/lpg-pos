using LpgSalesApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LpgSalesApp.UI.ViewModels;

namespace LpgSalesApp.UI.Windows;

/// <summary>
/// Interaction logic for TransactionFormWindow.xaml
/// </summary>
public partial class TransactionFormWindow : Window
{
    public TransactionFormWindow()
    {
        InitializeComponent();
        var customerService = App.Services.GetService(typeof(ICustomerService)) as ICustomerService;
        var productService = App.Services.GetService(typeof(IProductService)) as IProductService;
        var transactionService = App.Services.GetService(typeof(ITransactionService)) as ITransactionService;
        
        this.DataContext = new TransactionFormViewModel(customerService!, productService!, transactionService!, this.Close);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ProductSearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is TransactionFormViewModel vm)
            {
                vm.SelectFirstProductMatch();
            }
        }
    }

    private void CustomerSearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is TransactionFormViewModel vm)
            {
                vm.SelectFirstCustomerMatch();
            }
        }
    }
}
