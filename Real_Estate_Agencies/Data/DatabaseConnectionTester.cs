using System;
using System.Windows;

namespace Real_Estate_Agencies.Data
{

    public static class DatabaseConnectionTester
    {
        public static void ShowConnectionStatus()
        {
            string currentStatus = DatabaseConnectionManager.GetConnectionStatus();
            var (onlineAvailable, localAvailable, testMessage) = DatabaseConnectionManager.TestConnections();

            string message = $"Current Active Connection: {currentStatus}\n\n";
            message += "Connection Test Results:\n";
            message += testMessage;
            message += "\n";

            if (onlineAvailable && localAvailable)
            {
                message += "✓ Both databases are accessible";
            }
            else if (onlineAvailable)
            {
                message += "⚠ Only online database is accessible";
            }
            else if (localAvailable)
            {
                message += "⚠ Only local database is accessible";
            }
            else
            {
                message += "✗ No database connection available!";
            }

            MessageBox.Show(message, "Database Connection Status", 
                MessageBoxButton.OK, 
                onlineAvailable || localAvailable ? MessageBoxImage.Information : MessageBoxImage.Error);
        }

        public static void SwitchToLocalDatabase()
        {
            DatabaseConnectionManager.ForceLocalDatabase();
            MessageBox.Show("Switched to local database.\n\nThe application will now use the local database for all operations.",
                "Database Switch", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void SwitchToOnlineDatabase()
        {
            DatabaseConnectionManager.ForceOnlineDatabase();
            MessageBox.Show("Attempting to reconnect to online database.\n\nThe application will try to use the online database for the next operation.",
                "Database Switch", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static string GetSimpleStatus()
        {
            string status = DatabaseConnectionManager.GetConnectionStatus();
            return $"Database: {status}";
        }

        public static string GetStatusIcon()
        {
            string status = DatabaseConnectionManager.GetConnectionStatus();
            return status.Contains("Online") ? "🌐" : "💾";
        }
    }
}
