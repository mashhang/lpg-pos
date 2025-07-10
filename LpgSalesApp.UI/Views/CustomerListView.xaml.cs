using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
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

namespace LpgSalesApp.UI.Views;

/// <summary>
/// Interaction logic for CustomerListView.xaml
/// </summary>
public partial class CustomerListView : UserControl
{
    public CustomerListView()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        var service = App.Services.GetService(typeof(ICustomerService)) as ICustomerService;
        this.DataContext = new CustomerListViewModel(service!);
    }
}
