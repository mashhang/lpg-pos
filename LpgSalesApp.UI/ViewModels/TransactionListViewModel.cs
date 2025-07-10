using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LpgSalesApp.UI.ViewModels;

public class TransactionListViewModel : NotifyPropertyChanged
{
    private readonly ITransactionService _transactionService;

    public ObservableCollection<TransactionDto> Transactions { get; set; } = new();

    public ICommand RefreshCommand { get; }
    public ICommand AddCommand { get; }

    public TransactionListViewModel(ITransactionService transactionService)
    {
        _transactionService = transactionService;

        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        AddCommand = new RelayCommand(_ => OpenNewTransaction());

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var data = await _transactionService.GetAllAsync();
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            Transactions.Clear();
            foreach (var item in data)
                Transactions.Add(item);
        });
    }

    private void OpenNewTransaction()
    {
        var win = new Windows.TransactionFormWindow();
        win.ShowDialog();
        _ = LoadAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
