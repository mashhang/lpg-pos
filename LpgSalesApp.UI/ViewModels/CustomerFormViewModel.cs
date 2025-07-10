using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LpgSalesApp.UI.ViewModels;

internal class CustomerFormViewModel : INotifyPropertyChanged
{
    private readonly ICustomerService _customerService;
    private readonly Action _onClose;

    private int? _editingId = null;

    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICommand SaveCommand { get; }

    public CustomerFormViewModel(ICustomerService service, Action onClose, CustomerDto? existing = null)
    {
        _customerService = service;
        _onClose = onClose;
        SaveCommand = new RelayCommand(async _ => await SaveAsync());

        if (existing != null)
        {
            _editingId = existing.Id;
            FullName = existing.FullName;
            ContactNumber = existing.ContactNumber;
            Address = existing.Address;
        }
    }

    private async Task SaveAsync()
    {
        var dto = new CustomerDto
        {
            Id = _editingId ?? 0,
            FullName = this.FullName,
            ContactNumber = this.ContactNumber,
            Address = this.Address
        };

        if (_editingId.HasValue)
        {
            await _customerService.UpdateAsync(dto);
        }
        else
            await _customerService.CreateAsync(dto);

        _onClose();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
