using LpgSalesApp.Application.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace LpgSalesApp.UI.Views.Admin;

public partial class EditUserWindow : Window
{
    public string UpdatedUsername { get; private set; }
    public string? UpdatedPassword { get; private set; }
    public string UpdatedRole { get; private set; }

    public EditUserWindow(UserDto user)
    {
        InitializeComponent();
        UsernameBox.Text = user.Username;
        RoleBox.SelectedItem = RoleBox.Items
            .OfType<ComboBoxItem>()
            .FirstOrDefault(i => i.Content.ToString() == user.Role);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        UpdatedUsername = UsernameBox.Text.Trim();

        UpdatedRole = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

        if (string.IsNullOrWhiteSpace(UpdatedUsername) || string.IsNullOrWhiteSpace(UpdatedRole) || string.IsNullOrWhiteSpace(UpdatedPassword))
        {
            MessageBox.Show("All fields are required.");
            return;
        }

        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
