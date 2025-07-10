using CommunityToolkit.Mvvm.Input;
using LpgSalesApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LpgSalesApp.Application.DTOs;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace LpgSalesApp.UI.ViewModels;

public class CustomerListViewModel : INotifyPropertyChanged
{
    private readonly ICustomerService _customerService;


    public ObservableCollection<CustomerDto> Customers { get; set; } = new();

    private CustomerDto? _selectedCustomer;
    public CustomerDto? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            _selectedCustomer = value;
            OnPropertyChanged();
            // This tells WPF to re-check CanExecute and IsEnabled
            (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public ICommand RefreshCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public CustomerListViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        RefreshCommand = new RelayCommand(async _ => await LoadCustomersAsync());
        AddCommand = new RelayCommand(_ => OpenCustomerForm());
        EditCommand = new RelayCommand(_ => OpenEditForm(), _ => SelectedCustomer != null);
        DeleteCommand = new RelayCommand(async _ => await DeleteCustomerAsync(), _ => SelectedCustomer != null);

        _ = LoadCustomersAsync();
    }

    private void OpenEditForm()
    {
        var window = new Windows.CustomerFormWindow(SelectedCustomer);
        window.ShowDialog();
        _ = LoadCustomersAsync();
    }

    private async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer == null) return;

        var result = MessageBox.Show(
            $"Are you sure you want to delete {SelectedCustomer.FullName}?",
            "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            await _customerService.DeleteAsync(SelectedCustomer.Id);
            await LoadCustomersAsync();
        }
    }

    private async Task LoadCustomersAsync()
    {
        var data = await _customerService.GetAllAsync();
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            Customers.Clear();
            foreach (var customer in data)
                Customers.Add(customer);
        });
    }

    private void OpenCustomerForm()
    {
        var window = new Windows.CustomerFormWindow();
        window.ShowDialog();
        _ = LoadCustomersAsync(); // refresh after closing form
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
