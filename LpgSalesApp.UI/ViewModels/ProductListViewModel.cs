using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LpgSalesApp.UI.ViewModels;

public class ProductListViewModel : INotifyPropertyChanged
{
    private readonly IProductService _productService;

    public ObservableCollection<ProductDto> Products { get; set; } = new();
    public ProductDto? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
            (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    private ProductDto? _selectedProduct;

    public ICommand RefreshCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public ProductListViewModel(IProductService productService)
    {
        _productService = productService;

        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        AddCommand = new RelayCommand(_ => OpenAdd());
        EditCommand = new RelayCommand(_ => OpenEdit(), _ => SelectedProduct != null);
        DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedProduct != null);

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _productService.GetAllAsync();
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            Products.Clear();
            foreach (var item in items)
                Products.Add(item);
        });
    }

    private void OpenAdd()
    {
        var win = new Windows.ProductFormWindow();
        win.ShowDialog();
        _ = LoadAsync();
    }

    private void OpenEdit()
    {
        var win = new Windows.ProductFormWindow(SelectedProduct);
        win.ShowDialog();
        _ = LoadAsync();
    }

    private async Task DeleteAsync()
    {
        if (SelectedProduct == null) return;
        var confirm = MessageBox.Show($"Delete {SelectedProduct.Name}?", "Confirm", MessageBoxButton.YesNo);
        if (confirm == MessageBoxResult.Yes)
        {
            await _productService.DeleteAsync(SelectedProduct.Id);
            await LoadAsync();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
