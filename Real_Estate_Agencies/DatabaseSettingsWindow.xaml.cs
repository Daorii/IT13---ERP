using System;
using System.Windows;
using Real_Estate_Agencies.Data;

namespace Real_Estate_Agencies
{
    public partial class DatabaseSettingsWindow : Window
    {
        public DatabaseSettingsWindow()
        {
            InitializeComponent();
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            string currentStatus = DatabaseConnectionManager.GetConnectionStatus();
            string icon = currentStatus.Contains("Online") ? "🌐" : "💾";
            StatusTextBlock.Text = $"Active: {currentStatus} {icon}";
        }

        private void TestConnections_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var (onlineAvailable, localAvailable, message) = DatabaseConnectionManager.TestConnections();

                TestResultsTextBlock.Text = message;

                // Update status after test
                RefreshStatus();

                // Show summary message
                string summary = "";
                if (onlineAvailable && localAvailable)
                {
                    summary = "✓ Both databases are accessible and working properly!";
                }
                else if (onlineAvailable)
                {
                    summary = "⚠ Only online database is accessible. Local database connection failed.";
                }
                else if (localAvailable)
                {
                    summary = "⚠ Only local database is accessible. Online database connection failed.";
                }
                else
                {
                    summary = "✗ Both database connections failed! Please check your configuration.";
                }

                MessageBox.Show(summary, "Connection Test Complete",
                    MessageBoxButton.OK,
                    onlineAvailable || localAvailable ? MessageBoxImage.Information : MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error testing connections: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UseOnline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseConnectionManager.ForceOnlineDatabase();
                RefreshStatus();
                
                MessageBox.Show("Switched to online database.\n\nThe application will attempt to use the online database for all operations.",
                    "Database Switch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to online database: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UseLocal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseConnectionManager.ForceLocalDatabase();
                RefreshStatus();
                
                MessageBox.Show("Switched to local database.\n\nThe application will use the local database for all operations.",
                    "Database Switch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error switching to local database: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshStatus_Click(object sender, RoutedEventArgs e)
        {
            RefreshStatus();
            MessageBox.Show("Status refreshed successfully!",
                "Refresh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
