using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LpgSalesApp.Application.DTOs;
using CommunityToolkit.Mvvm.Input;
using LpgSalesApp.Application.Interfaces;
using LpgSalesApp.UI.Views.Auth;

namespace LpgSalesApp.UI.ViewModels;

internal class LoginViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly Action _onSuccess;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUserService userService, Action onSuccess)
    {
        _userService = userService;
        _onSuccess = onSuccess;
        LoginCommand = new RelayCommand<object?>(Login);
    }

    private void Login(object? _)
    {
        var user = _userService.Authenticate(Username, Password);
        if (user != null)
        {
            App.CurrentUser = user;
            _onSuccess?.Invoke();
        }
        else
        {
            MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
