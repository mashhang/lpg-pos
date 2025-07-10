using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.UI.Windows;

/// <summary>
/// Interaction logic for CustomerFormWindow.xaml
/// </summary>
public partial class CustomerFormWindow : Window
{
    public CustomerFormWindow(CustomerDto? existing = null)
    {
        InitializeComponent();

        var service = App.Services.GetService(typeof(ICustomerService)) as ICustomerService;
        var viewModel = new CustomerFormViewModel(service!, this.Close, existing);
        this.DataContext = viewModel;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
