using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class LogsPage : Page
    {
        private readonly CommissionReleaseRepository _repository;
        private List<CommissionReleaseModel> allLogs;

        public LogsPage()
        {
            InitializeComponent();
            _repository = new CommissionReleaseRepository();
            LoadLogs();

            // Hook up search events
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            SearchTextBox.LostFocus += SearchTextBox_LostFocus;
        }

        private void LoadLogs()
        {
            try
            {
                allLogs = _repository.GetAll() ?? new List<CommissionReleaseModel>();
                LogsDataGrid.ItemsSource = allLogs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logs: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TogglePlaceholder();

            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                LogsDataGrid.ItemsSource = allLogs;
                return;
            }

            var filtered = allLogs.Where(log =>
                log.ReleaseID.ToString().Contains(searchText) ||
                log.AgentName.ToLower().Contains(searchText) ||
                log.ReleaseDate.ToString("yyyy-MM-dd").Contains(searchText) ||
                log.ReleasedAmount.ToString("C").Contains(searchText)
            ).ToList();

            LogsDataGrid.ItemsSource = filtered;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TogglePlaceholder();
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TogglePlaceholder();
        }

        private void TogglePlaceholder()
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceholderText.Visibility = Visibility.Collapsed;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CommissionsPage commissionPage = new CommissionsPage();
            this.NavigationService?.Navigate(commissionPage);
        }
    }
}
