using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

    private void WindowHeader_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    // Allows only numeric input (and a single decimal point for price)
    private void PriceStockTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        // Use a Regex to allow only numbers and one decimal point
        Regex regex = new Regex("^[0-9]+([.][0-9]+)?$"); // For price
        if (textBox.Name == "StockTextBox")
        {
            regex = new Regex("^[0-9]*$"); // For stock (integers only)
        }

        // If the new text would not match the regex, cancel the input
        string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
        e.Handled = !regex.IsMatch(newText);
    }

    // Prevents pasting non-numeric text
    private void PriceStockTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            string text = (string)e.DataObject.GetData(typeof(string));
            TextBox textBox = sender as TextBox;
            Regex regex = new Regex("^[0-9]+([.][0-9]+)?$");
            if (textBox.Name == "StockTextBox")
            {
                regex = new Regex("^[0-9]*$");
            }

            if (!regex.IsMatch(text))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    // --- Price Spinner Clicks ---
    private void PriceUp_Click(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(PriceTextBox.Text, out double price))
        {
            PriceTextBox.Text = (price + 1.00).ToString("F2");
        }
        else
        {
            PriceTextBox.Text = "1.00"; // Default if empty or invalid
        }
    }

    private void PriceDown_Click(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(PriceTextBox.Text, out double price))
        {
            if (price > 0)
            {
                PriceTextBox.Text = (price - 1.00).ToString("F2");
            }
            else
            {
                PriceTextBox.Text = "0.00"; // Don't go below zero
            }
        }
        else
        {
            PriceTextBox.Text = "0.00"; // Default if empty or invalid
        }
    }

    // --- Stock Spinner Clicks ---
    private void StockUp_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(StockTextBox.Text, out int stock))
        {
            StockTextBox.Text = (stock + 1).ToString();
        }
        else
        {
            StockTextBox.Text = "1"; // Default if empty or invalid
        }
    }

    private void StockDown_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(StockTextBox.Text, out int stock))
        {
            if (stock > 0)
            {
                StockTextBox.Text = (stock - 1).ToString();
            }
            else
            {
                StockTextBox.Text = "0"; // Don't go below zero
            }
        }
        else
        {
            StockTextBox.Text = "0"; // Default if empty or invalid
        }
    }

    // Handle LostFocus to format price to 2 decimal places and ensure 0 for stock
    private void PriceTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(PriceTextBox.Text, out double price))
        {
            PriceTextBox.Text = price.ToString("F2");
        }
        else if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
        {
            PriceTextBox.Text = "0.00";
        }
    }

    private void StockTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(StockTextBox.Text, out int stock))
        {
            StockTextBox.Text = stock.ToString();
        }
        else if (string.IsNullOrWhiteSpace(StockTextBox.Text))
        {
            StockTextBox.Text = "0";
        }
    }

    // Optional: Hide watermark when text is entered
    private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            // This will trigger the style's data trigger to hide/show the watermark
            // No direct code needed here unless you want more complex logic.
        }
    }
}
