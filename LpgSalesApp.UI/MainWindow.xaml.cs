using LpgSalesApp.UI.Views;
using LpgSalesApp.UI.Views.Reports; // Make sure this is correct for your Reports.TransactionListView
using System.Windows;
using System.Windows.Controls;

namespace LpgSalesApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the initial view to Products when the window loads
            MainContentArea.Content = new ProductListView();

            // Attach event handlers to RadioButtons for navigation using direct access
            // Now that NavButtonsPanel has x:Name="NavButtonsPanel", we can access it directly.
            foreach (var child in NavButtonsPanel.Children)
            {
                if (child is RadioButton rb)
                {
                    rb.Checked += NavRadioButton_Checked;
                }
            }
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string tag = rb.Tag as string;
                switch (tag)
                {
                    case "Products":
                        MainContentArea.Content = new ProductListView();
                        break;
                    case "Customers":
                        MainContentArea.Content = new CustomerListView();
                        break;
                    case "Transactions":
                        MainContentArea.Content = new Views.TransactionListView();
                        break;
                    case "History":
                        // Ensure this path is correct if TransactionListView is in a sub-namespace
                        MainContentArea.Content = new Views.Reports.TransactionListView();
                        break;
                        // Add more cases for other tabs if needed
                }
            }
        }
    }
}