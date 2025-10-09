using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using Real_Estate_Agencies.Data;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class LogsPage : Page
    {
        private readonly CommissionReleaseRepository _repository;

        public LogsPage()
        {
            InitializeComponent();
            _repository = new CommissionReleaseRepository();
            LoadLogs();
        }

        private void LoadLogs()
        {
            try
            {
                var logs = _repository.GetAll();
                LogsDataGrid.ItemsSource = logs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logs: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CommissionReleaseModel log)
            {
                MessageBox.Show(
                    $"Release ID: {log.ReleaseID}\n" +
                    $"Agent Name: {log.AgentName}\n" +
                    $"Release Date: {log.ReleaseDate:d}",
                    "Log Details",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CommissionsPage commissionPage = new CommissionsPage();
            this.NavigationService?.Navigate(commissionPage);
        }
    }
}
