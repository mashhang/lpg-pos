using CommunityToolkit.Mvvm.Input;
using LpgSalesApp.Application.DTOs;
using LpgSalesApp.Application.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LpgSalesApp.UI.ViewModels.Admin;

public class UserManagementViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    
    public ObservableCollection<UserDto> Users { get; set; } = new();

    private UserDto? _selectedUser;
    public UserDto? SelectedUser
    {
        get => _selectedUser;
        set
        {
            SetProperty(ref _selectedUser, value);
            if (value != null)
            {
                Username = value.Username;
                Role = value.Role;
                Password = string.Empty; // Optional password reset
            }
        }
    }
    public ICommand CancelEditCommand { get; }
    public IAsyncRelayCommand SaveUserCommand { get; }


    private string _username;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _role = "User"; // Default role
    public string Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }

    public IAsyncRelayCommand LoadUsersCommand { get; }
    public IAsyncRelayCommand CreateUserCommand { get; }

    public ICommand EditUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    private string _editablePassword;
    public string EditablePassword
    {
        get => _editablePassword;
        set => SetProperty(ref _editablePassword, value);
    }

    public UserManagementViewModel(IUserService userService)
    {
        _userService = userService;

        SaveUserCommand = new AsyncRelayCommand(SaveUserAsync);
        LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
        CreateUserCommand = new AsyncRelayCommand(CreateUserAsync);
        //EditUserCommand = new RelayCommand<UserDto>(OnEditUser);
        CancelEditCommand = new RelayCommand<object?>(_ => CancelEdit());
        DeleteUserCommand = new RelayCommand<UserDto>(OnDeleteUser);

        _ = LoadUsersAsync(); // Auto-load
    }

    private async Task LoadUsersAsync()
    {
        Users.Clear();
        var users = await _userService.GetAllUsersAsync();
        foreach (var user in users)
            Users.Add(user);
    }

    private async Task CreateUserAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) return;

        await _userService.CreateUserAsync(Username, Password, Role);
        Username = Password = string.Empty;
        await LoadUsersAsync();
    }


    //private async void OnEditUser(UserDto user)
    //{
    //    //Username = user.Username;
    //    //Role = user.Role;
    //    //SelectedUser = user;
    //    var editWindow = new Views.Admin.EditUserWindow(user);
    //    var result = editWindow.ShowDialog();

    //    if (result == true)
    //    {
    //        var newUsername = editWindow.UpdatedUsername;
    //        var newRole = editWindow.UpdatedRole;
    //        var newPassword = editWindow.UpdatedPassword;

    //        await _userService.UpdateUserAsync(user.Id, newUsername, newRole, newPassword);
    //        await LoadUsersAsync();
    //    }
    //}

    private async void OnDeleteUser(UserDto user)
    {
        if (MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            await _userService.DeleteUserAsync(user.Id);
            await LoadUsersAsync();
        }
    }

    private async Task SaveUserAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
            return;

        if (SelectedUser == null)
        {
            await _userService.CreateUserAsync(Username, Password, Role);
        }
        else
        {
            await _userService.UpdateUserAsync(SelectedUser.Id, Username, Role, string.IsNullOrWhiteSpace(Password) ? null : Password);
        }

        await LoadUsersAsync();
        CancelEdit(); // Reset form
    }

    private void CancelEdit()
    {
        SelectedUser = null;
        Username = string.Empty;
        Password = string.Empty;
        Role = "User";
    }
}
