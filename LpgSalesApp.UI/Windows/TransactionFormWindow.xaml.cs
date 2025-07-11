using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        // For the custom window drag functionality
        this.MouseLeftButtonDown += WindowHeader_MouseDown;
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

    private void WindowHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    // Event handler for numeric input validation on Quantity TextBox
    private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Allow only digits (0-9)
        e.Handled = !IsTextAllowed(e.Text);
    }
    // Event handler for pasting into Quantity TextBox
    private void Quantity_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            string text = (string)e.DataObject.GetData(typeof(string));
            if (!IsTextAllowed(text))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    // Helper method to check if text is numeric
    private bool IsTextAllowed(string text)
    {
        // Regex for numbers (integers only for quantity)
        // If you need decimal quantities, you'd adjust this regex to include '\.'
        Regex regex = new Regex("[^0-9]+"); // Disallow non-numeric characters
        return !regex.IsMatch(text);
    }

    private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(QuantityTextBox.Text, out int qty))
            QuantityTextBox.Text = (qty + 1).ToString();
    }

    private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(QuantityTextBox.Text, out int qty) && qty > 1)
            QuantityTextBox.Text = (qty - 1).ToString();
    }
}
