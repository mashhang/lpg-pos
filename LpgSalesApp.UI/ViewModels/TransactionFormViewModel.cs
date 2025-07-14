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
using System.Windows.Data;
using System.Windows.Input;

namespace LpgSalesApp.UI.ViewModels;

public class TransactionFormViewModel : INotifyPropertyChanged
{
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ITransactionService _transactionService;
    private readonly Action _onClose;

    public ObservableCollection<CustomerDto> Customers { get; set; } = new();
    public ObservableCollection<ProductDto> Products { get; set; } = new();
    public ObservableCollection<TransactionItemDto> Items { get; set; } = new();

    public CustomerDto? SelectedCustomer { get; set; }
    public ProductDto? SelectedProduct { get; set; }
    public int Quantity { get; set; } = 1;

    public decimal TotalAmount => Items.Sum(i => i.Subtotal);

    private string _remarks;
    public string Remarks
    {
        get => _remarks;
        set
        {
            _remarks = value;
            OnPropertyChanged(nameof(Remarks));
        }
    }

    public ICommand AddItemCommand { get; }
    public ICommand SaveCommand { get; }

    public ICollectionView ProductView { get; set; }
    public ICollectionView CustomerView { get; set; }

    public TransactionFormViewModel(ICustomerService customerService, IProductService productService,
        ITransactionService transactionService, Action onClose)
    {
        _customerService = customerService;
        _productService = productService;
        _transactionService = transactionService;
        _onClose = onClose;

        AddItemCommand = new RelayCommand(_ => AddItem());
        SaveCommand = new RelayCommand(async _ => await SaveAsync());

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var customers = await _customerService.GetAllAsync();
        var products = await _productService.GetAllAsync();

        Customers.Clear();
        foreach (var c in customers)
            Customers.Add(c);

        Products.Clear();
        foreach (var p in products)
            Products.Add(p);

        ProductView = CollectionViewSource.GetDefaultView(Products);
        ProductView.Filter = o =>
        {
            if (o is not ProductDto p) return false;
            return string.IsNullOrWhiteSpace(ProductSearchText) || p.Name.Contains(ProductSearchText, StringComparison.OrdinalIgnoreCase);
        };

        CustomerView = CollectionViewSource.GetDefaultView(Customers);
        CustomerView.Filter = o =>
        {
            if (o is not CustomerDto c) return false;
            return string.IsNullOrWhiteSpace(CustomerSearchText) || c.FullName.Contains(CustomerSearchText, StringComparison.OrdinalIgnoreCase);
        };

        OnPropertyChanged(nameof(ProductView));
        OnPropertyChanged(nameof(CustomerView));
    }

    private void AddItem()
    {
        if (SelectedProduct == null || Quantity <= 0) return;

        var item = new TransactionItemDto
        {
            ProductId = SelectedProduct.Id,
            ProductName = SelectedProduct.Name,
            UnitPrice = SelectedProduct.Price,
            Quantity = Quantity,
            Subtotal = SelectedProduct.Price * Quantity
        };

        Items.Add(item);
        //TotalAmount = Items.Sum(i => i.Subtotal);
        OnPropertyChanged(nameof(TotalAmount));

        // ✅ Auto-clear search & quantity:
        ProductSearchText = string.Empty;
        Quantity = 1;
        SelectedProduct = null;

        ProductView.Refresh();
    }

    private async Task SaveAsync()
    {
        if (SelectedCustomer == null || Items.Count == 0) return;

        var dto = new TransactionDto
        {
            CustomerId = SelectedCustomer.Id,
            Items = Items.ToList(),
            TransactionDate = DateTime.Now,
            TotalAmount = TotalAmount,
            Remarks = Remarks,
        };

        await _transactionService.CreateAsync(dto);
        _onClose();
    }

    public string ProductSearchText
    {
        get => _productSearchText;
        set
        {
            _productSearchText = value;
            OnPropertyChanged();
            ProductView.Refresh();
        }
    }
    private string _productSearchText = string.Empty;

    public string CustomerSearchText
    {
        get => _customerSearchText;
        set
        {
            _customerSearchText = value;
            OnPropertyChanged();
            CustomerView.Refresh();
        }
    }
    private string _customerSearchText = string.Empty;

    public void SelectFirstProductMatch()
    {
        if (ProductView.Cast<object>().FirstOrDefault() is ProductDto firstMatch)
        {
            SelectedProduct = firstMatch;
            OnPropertyChanged(nameof(SelectedProduct));
        }
    }

    public void SelectFirstCustomerMatch()
    {
        if (CustomerView.Cast<object>().FirstOrDefault() is CustomerDto firstMatch)
        {
            SelectedCustomer = firstMatch;
            OnPropertyChanged(nameof(SelectedCustomer));
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
