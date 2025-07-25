﻿using LpgSalesApp.UI.Views;
using LpgSalesApp.UI.Views.Auth;
using LpgSalesApp.UI.Views.Admin;
using LpgSalesApp.UI.ViewModels.Admin;
using LpgSalesApp.UI.Views.Reports; // Make sure this is correct for your Reports.TransactionListView
using System.Windows;
using System.Windows.Controls;
using LpgSalesApp.Application.DTOs;

namespace LpgSalesApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserDto _currentUser;

        public MainWindow(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            this.Loaded += MainWindow_Loaded;

            // Show Users tab only for Admins
            if (_currentUser.Role == "Admin")
                ManageUsersTab.Visibility = Visibility.Visible;

            // Register radio button events
            foreach (var child in NavButtonsPanel.Children)
            {
                if (child is RadioButton radio)
                    radio.Checked += NavRadioButton_Checked;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the initial view to Products when the window loads
            MainContentArea.Content = new ProductListView();

            // Attach event handlers to RadioButtons for navigation using direct access
            // Now that NavButtonsPanel has x:Name="NavButtonsPanel", we can access it directly.
            foreach (var child in NavButtonsPanel.Children)
            {
                if (child is RadioButton rb)
                {
                    rb.Checked += NavRadioButton_Checked;
                }
            }
        }

        public void NavigateToPage(UserControl view)
        {
            MainContentArea.Content = view;
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string tag = rb.Tag as string;
                switch (tag)
                {
                    case "Products":
                        MainContentArea.Content = new ProductListView();
                        break;
                    case "Customers":
                        MainContentArea.Content = new CustomerListView();
                        break;
                    case "Transactions":
                        MainContentArea.Content = new Views.TransactionListView();
                        break;
                    case "History":
                        // Ensure this path is correct if TransactionListView is in a sub-namespace
                        MainContentArea.Content = new Views.Reports.TransactionListView();
                        break;
                    case "Users":
                        MainContentArea.Content = new Views.Admin.UserManagementView();
                        break;
                }
            }
        }

        public static class SessionManager
        {
            public static string CurrentUser { get; set; }

            public static void Clear()
            {
                CurrentUser = null;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Optional: Clear session variables or static user info
            SessionManager.Clear();

            // Open login window
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Close the current main window
            this.Close();
        }
    }
}