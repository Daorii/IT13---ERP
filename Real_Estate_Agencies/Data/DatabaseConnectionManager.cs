using System;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public static class DatabaseConnectionManager
    {

        private static readonly string _onlineConnectionString =
            "Data Source=db29561.public.databaseasp.net;User ID=db29561;Password=123456789;Encrypt=False;Trust Server Certificate=True";

         private static readonly string _localConnectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=RealEstateDB;Integrated Security=True;Connect Timeout=5";

        private static bool _isOnlineAvailable = true;
        private static DateTime _lastCheckTime = DateTime.MinValue;
        private static readonly TimeSpan _recheckInterval = TimeSpan.FromMinutes(5); // Recheck online DB every 5 minutes

        public static string GetConnectionString()
        {
            // If enough time has passed since last check, try online again
            if (!_isOnlineAvailable && (DateTime.Now - _lastCheckTime) > _recheckInterval)
            {
                _isOnlineAvailable = true; // Reset to try online again
            }

            if (_isOnlineAvailable)
            {
                return _onlineConnectionString;
            }
            else
            {
                return _localConnectionString;
            }
        }

        public static SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                
                // If we successfully connected and we were previously using local, mark online as available
                if (connectionString == _onlineConnectionString && !_isOnlineAvailable)
                {
                    _isOnlineAvailable = true;
                    ShowConnectionStatusMessage("Reconnected to online database successfully.");
                }
                
                return connection;
            }
            catch (SqlException ex)
            {
                // If online connection failed, try local
                if (connectionString == _onlineConnectionString)
                {
                    _isOnlineAvailable = false;
                    _lastCheckTime = DateTime.Now;
                    
                    ShowConnectionStatusMessage($"Online database unavailable. Switching to local database.\nError: {ex.Message}");

                    try
                    {
                        connection.Dispose();
                        connection = new SqlConnection(_localConnectionString);
                        connection.Open();
                        return connection;
                    }
                    catch (SqlException localEx)
                    {
                        connection.Dispose();
                        throw new Exception($"Both online and local databases are unavailable.\nOnline Error: {ex.Message}\nLocal Error: {localEx.Message}");
                    }
                }
                else
                {
                    // Local connection also failed
                    connection.Dispose();
                    throw new Exception($"Local database connection failed: {ex.Message}");
                }
            }
        }

        public static (bool onlineAvailable, bool localAvailable, string message) TestConnections()
        {
            bool onlineAvailable = false;
            bool localAvailable = false;
            string message = "";

            // Test online database
            try
            {
                using (SqlConnection conn = new SqlConnection(_onlineConnectionString))
                {
                    conn.Open();
                    onlineAvailable = true;
                    message += "✓ Online database: Connected\n";
                }
            }
            catch (Exception ex)
            {
                message += $"✗ Online database: Failed ({ex.Message})\n";
            }

            // Test local database
            try
            {
                using (SqlConnection conn = new SqlConnection(_localConnectionString))
                {
                    conn.Open();
                    localAvailable = true;
                    message += "✓ Local database: Connected\n";
                }
            }
            catch (Exception ex)
            {
                message += $"✗ Local database: Failed ({ex.Message})\n";
            }

            return (onlineAvailable, localAvailable, message);
        }

        public static void ForceLocalDatabase()
        {
            _isOnlineAvailable = false;
            _lastCheckTime = DateTime.Now;
        }

        public static void ForceOnlineDatabase()
        {
            _isOnlineAvailable = true;
        }

        public static string GetConnectionStatus()
        {
            return _isOnlineAvailable ? "Online Database" : "Local Database";
        }

        private static void ShowConnectionStatusMessage(string message)
        {
            // Show message in a non-blocking way
            Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(message, "Database Connection Status", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }));
        }
    }
}
