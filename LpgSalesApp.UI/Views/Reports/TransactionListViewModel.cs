//using CommunityToolkit.Mvvm.Input;
//using LpgSalesApp.Application.DTOs;
//using LpgSalesApp.Application.Interfaces;
//using LpgSalesApp.Infrastructure.Services;
//using LpgSalesApp.UI.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;

//namespace LpgSalesApp.UI.Views.Reports
//{
//    public class TransactionListViewModel : ViewModelBase
//    {
//        private readonly ITransactionService _transactionService;

//        public ObservableCollection<TransactionDto> Transactions { get; set; } = new();

//        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);
//        public DateTime EndDate { get; set; } = DateTime.Today;

//        public ICommand LoadCommand { get; }
//        public ICommand ViewDetailsCommand { get; }

//        public TransactionDto? SelectedTransaction { get; set; }

//        public TransactionListViewModel(ITransactionService tansactionService)
//        {
//            _transactionService = tansactionService;

//            LoadCommand = new RelayCommand(async () => await LoadAsync());
//            ViewDetailsCommand = new RelayCommand(ViewDetails, () => SelectedTransaction != null);

//        }

//        private async Task LoadAsync()
//        {
//            Transactions.Clear();
//            var data = await _transactionService.GetByDateRangeAsync(StartDate, EndDate);
//            foreach (var t in data)
//                Transactions.Add(t);
//        }

//        private void ViewDetails()
//        {
//            if (SelectedTransaction != null) return;

//            //var window = new TransactionDetailsWindow(SelectedTransaction);
//            //window.ShowDialog();
//        }
//    }
//}
using CommunityToolkit.Mvvm.Input;
using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;

namespace LpgSalesApp.UI.Views.Reports;

public class TransactionListViewModel : ViewModelBase
{
    private readonly ITransactionService _transactionService;

    public ObservableCollection<TransactionDto> Transactions { get; set; } = new();

    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);
    public DateTime EndDate { get; set; } = DateTime.Today;

    private TransactionDto? _selectedTransaction;
    public TransactionDto? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            _selectedTransaction = value;
            OnPropertyChanged(nameof(SelectedTransaction));
            ((RelayCommand)ViewDetailsCommand).NotifyCanExecuteChanged();
        }
    }

    public ICommand LoadCommand { get; }
    public ICommand ViewDetailsCommand { get; }

    public TransactionListViewModel(ITransactionService transactionService)
    {
        _transactionService = transactionService;

        LoadCommand = new RelayCommand(async () => await LoadAsync());
        ViewDetailsCommand = new RelayCommand(ViewDetails, () => SelectedTransaction != null);
    }

    public async Task LoadAsync()
    {
        Transactions.Clear();
        var data = await _transactionService.GetByDateRangeAsync(StartDate, EndDate);
        foreach (var t in data)
            Transactions.Add(t);
    }

    private void ViewDetails()
    {
        if (SelectedTransaction == null)
            return;

        // Placeholder - You can open TransactionDetailsWindow here later
        //MessageBox.Show($"View details for Transaction #{SelectedTransaction.Id}", "Transaction Details");
        var window = new TransactionDetailsWindow(SelectedTransaction); // You must implement this
        window.ShowDialog();
    }

}