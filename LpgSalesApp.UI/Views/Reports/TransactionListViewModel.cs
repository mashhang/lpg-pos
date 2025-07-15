
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
    private List<TransactionDto> _allTransactions = new();

    public ObservableCollection<TransactionDto> Transactions { get; set; } = new();

    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);
    public DateTime EndDate { get; set; } = DateTime.Today;

    private string _customerSearchText = string.Empty;
    public string CustomerSearchText
    {
        get => _customerSearchText;
        set
        {
            _customerSearchText = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }

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
        //foreach (var t in data)
        //    Transactions.Add(t);
        _allTransactions = data.ToList();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(CustomerSearchText)
            ? _allTransactions
            : _allTransactions
                .Where(t => !string.IsNullOrWhiteSpace(t.CustomerName) &&
                            t.CustomerName.Contains(CustomerSearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

        Transactions.Clear();
        foreach (var t in filtered)
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