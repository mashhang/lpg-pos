using LpgSalesApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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

namespace LpgSalesApp.UI.Views.Reports
{
    /// <summary>
    /// Interaction logic for TransactionListView.xaml
    /// </summary>
    public partial class TransactionListView : UserControl
    {
        public TransactionListView()
        {
            InitializeComponent();

            // Inject your service properly here
            var transactionService = App.Services.GetService<ITransactionService>();


            // Create the ViewModel and set it as DataContext
            this.DataContext = new TransactionListViewModel(transactionService);
        }

        // Optional: Handle Loaded event to auto-load data
        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is TransactionListViewModel vm)
            {
                await vm.LoadAsync();
            }
        }
    }
}
