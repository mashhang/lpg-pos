using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace LpgSalesApp.UI.ViewModels;

internal class ProductFormViewModel : INotifyPropertyChanged
{
    private readonly IProductService _productService;
    private readonly Action _onClose;
    private int? _editingId = null;

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public ICommand SaveCommand { get; }

    public ProductFormViewModel(IProductService service, Action onClose, ProductDto? existing = null)
    {
        _productService = service;
        _onClose = onClose;
        SaveCommand = new RelayCommand(async _ => await SaveAsync());

        if (existing != null)
        {
            _editingId = existing.Id;
            Name = existing.Name;
            Price = existing.Price;
            Stock = existing.Stock;
        }
    }

    private async Task SaveAsync()
    {
        var dto = new ProductDto
        {
            Id = _editingId ?? 0,
            Name = this.Name,
            Price = this.Price,
            Stock = this.Stock
        };

        if (_editingId.HasValue)
            await _productService.UpdateAsync(dto);
        else
            await _productService.CreateAsync(dto);

        _onClose();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
