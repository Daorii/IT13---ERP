using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Real_Estate_Agencies.Data;
using Microsoft.Data.SqlClient;

namespace Real_Estate_Agencies.Data
{
    public class DashboardRepository
    {
        public List<PropertyModel> GetTopSoldProperties(int topCount)
        {
            var list = new List<PropertyModel>();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {
                    string sql = @"
                        SELECT TOP (@TopCount)
                            p.PropertyID,
                            p.Name,
                            p.Location,
                            p.Type,
                            p.Category,
                            p.Price,
                            p.Status,
                            p.Image
                        FROM Properties p
                        INNER JOIN Sales s ON p.PropertyID = s.PropertyID
                        GROUP BY p.PropertyID, p.Name, p.Location, p.Type, p.Category, p.Price, p.Status, p.Image
                        ORDER BY COUNT(s.SaleID) DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TopCount", topCount);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new PropertyModel
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Location = reader.GetString(2),
                                    PropertyType = reader.GetString(3),
                                    Category = reader.GetString(4),
                                    Price = (double)reader.GetDecimal(5),
                                    Status = reader.GetString(6),
                                    ImagePath = reader.IsDBNull(7)
                                        ? null
                                        : ConvertImageToBase64((byte[])reader[7])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetTopSoldProperties): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        public List<AgentPerformanceModel> GetTopAgents(int topCount)
        {
            var list = new List<AgentPerformanceModel>();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {

                    string sql = @"
                        SELECT TOP (@TopCount)
                            a.AgentID,
                            a.FirstName + ' ' + a.LastName AS FullName,
                            COUNT(s.SaleID) AS TotalSales
                        FROM Agents a
                        LEFT JOIN Sales s ON a.AgentID = s.AgentID
                        GROUP BY a.AgentID, a.FirstName, a.LastName
                        ORDER BY TotalSales DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TopCount", topCount);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new AgentPerformanceModel
                                {
                                    AgentId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    TotalSales = reader.GetInt32(2),
                                    TotalRevenue = 0 // no revenue info in DB
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetTopAgents): {ex.Message}");
            }

            return list;
        }

        public Dictionary<string, int> GetPropertyStatusCounts()
        {
            var result = new Dictionary<string, int>
            {
                { "Available", 0 },
                { "Sold", 0 },
                { "Pending", 0 }
            };

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {

                    string sql = @"
                        SELECT Status, COUNT(*) AS Count
                        FROM Properties
                        GROUP BY Status";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader.GetString(0);
                                int count = reader.GetInt32(1);

                                if (result.ContainsKey(status))
                                    result[status] = count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetPropertyStatusCounts): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        public Dictionary<int, int> GetMonthlySalesCounts(int year)
        {
            var result = new Dictionary<int, int>();

            for (int i = 1; i <= 12; i++)
                result[i] = 0;

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {

                    string sql = @"
                        SELECT MONTH(SaleDate) AS Month, COUNT(*) AS SalesCount
                        FROM Sales
                        WHERE YEAR(SaleDate) = @Year
                        GROUP BY MONTH(SaleDate)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Year", year);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int month = reader.GetInt32(0);
                                int count = reader.GetInt32(1);
                                result[month] = count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetMonthlySalesCounts): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }


        // --- Get Agent Overview Stats ---
        // --- Get Agent Overview Stats ---
        public AgentStatsModel GetAgentStats(int agentId)
        {
            var stats = new AgentStatsModel();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {

                    // 1️⃣ Total sales (lifetime)
                    string sqlSales = @"
                SELECT COUNT(*) AS TotalSales
                FROM Sales
                WHERE AgentID = @AgentID";

                    using (SqlCommand cmd = new SqlCommand(sqlSales, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        stats.TotalSales = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                    }

                    // 2️⃣ Released Commission (all-time)
                    string sqlReleased = @"
                SELECT ISNULL(SUM(AmountReleased), 0)
                FROM CommissionReleases
                WHERE AgentID = @AgentID";

                    using (SqlCommand cmd = new SqlCommand(sqlReleased, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        stats.ReleasedCommission = Convert.ToDouble(cmd.ExecuteScalar() ?? 0);
                    }

                    // 3️⃣ Sales this month
                    string sqlDealsMonth = @"
                SELECT COUNT(*)
                FROM Sales
                WHERE AgentID = @AgentID
                  AND MONTH(SaleDate) = MONTH(GETDATE())
                  AND YEAR(SaleDate) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sqlDealsMonth, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        stats.DealsThisMonth = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                    }
                }

                // 4️⃣ Get current commission using AgentRepository (same as profile)
                var agentRepo = new AgentRepository();
                stats.CurrentCommission = Convert.ToDouble(agentRepo.GetAgentBalanceThisMonth(agentId));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAgentStats): {ex.Message}",
                    "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return stats;
        }








        // --- Get Agent Monthly Sales Data (for chart) ---
        public Dictionary<int, int> GetAgentMonthlySales(int agentId, int year)
        {
            var result = new Dictionary<int, int>();

            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"
            SELECT MONTH(SaleDate) AS Month, COUNT(*) AS SalesCount
            FROM Sales
            WHERE AgentID = @AgentID AND YEAR(SaleDate) = @Year
            GROUP BY MONTH(SaleDate)
            ORDER BY Month";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@AgentID", agentId);
                    cmd.Parameters.AddWithValue("@Year", year);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = reader.GetInt32(0);
                            int count = reader.GetInt32(1);
                            result[month] = count;
                        }
                    }
                }
            }

            // Ensure all 12 months exist
            for (int i = 1; i <= 12; i++)
                if (!result.ContainsKey(i))
                    result[i] = 0;

            return result;
        }


        // --- Get Recent Deals for an Agent ---
        public List<Deal> GetAgentRecentDeals(int agentId, int count = 5)
        {
            var deals = new List<Deal>();

            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"
            SELECT TOP (@Count)
                p.Name AS PropertyName,
                p.Price,
                s.SaleDate
            FROM Sales s
            INNER JOIN Properties p ON s.PropertyID = p.PropertyID
            WHERE s.AgentID = @AgentID
            ORDER BY s.SaleDate DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@AgentID", agentId);
                    cmd.Parameters.AddWithValue("@Count", count);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deals.Add(new Deal
                            {
                                Property = reader.GetString(0),
                                Price = $"{reader.GetDecimal(1):C}",
                                Date = reader.GetDateTime(2).ToString("yyyy-MM-dd")
                            });
                        }
                    }
                }
            }

            return deals;
        }


        public List<SaleRecord> GetRecentSales(int limit = 5)
        {
            var recentSales = new List<SaleRecord>();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {
                    string sql = @"
                SELECT TOP (@Limit)
                    ISNULL(p.Name, s.PropertyName) AS Property,
                    ISNULL(a.FirstName + ' ' + a.LastName, 'Unknown') AS Agent,
                    ISNULL(c.FirstName + ' ' + c.LastName, 'Unknown') AS Client,
                    s.SaleDate
                FROM Sales s
                LEFT JOIN Agents a ON s.AgentID = a.AgentID
                LEFT JOIN Clients c ON s.ClientID = c.ClientID
                LEFT JOIN Properties p ON s.PropertyID = p.PropertyID
                ORDER BY s.SaleDate DESC;";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Limit", limit);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                recentSales.Add(new SaleRecord
                                {
                                    Property = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                    Agent = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Client = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Date = reader.IsDBNull(3)
                                        ? ""
                                        : reader.GetDateTime(3).ToString("yyyy-MM-dd")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetRecentSales): {ex.Message}", "DB Error");
            }

            return recentSales;
        }



        // 🏠 1. Get count of available properties
        public int GetAvailablePropertiesCount()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Properties WHERE Status = 'Available'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }

        // 👨‍💼 2. Get count of agents
        public int GetAgentsCount()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Agents";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }

        // 💰 3. Get total commission released this month
        public decimal GetMonthlyCommissionTotal()
        {
            decimal total = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"
            SELECT ISNULL(SUM(CommissionAmount), 0)
            FROM Commissions
            WHERE MONTH(ReleaseDate) = MONTH(GETDATE())
              AND YEAR(ReleaseDate) = YEAR(GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                        total = Convert.ToDecimal(result);
                }
            }
            return total;
        }


        // 🏡 4. Get total number of properties (for Properties Listed card)
        public int GetTotalPropertiesCount()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Properties";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }



        // 🧮 Get total sales transactions for the current month
        public int GetMonthlySalesCount()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"
            SELECT COUNT(*) 
            FROM Sales
            WHERE MONTH(SaleDate) = MONTH(GETDATE())
              AND YEAR(SaleDate) = YEAR(GETDATE())";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }

        // 👨‍💼 Get total active agents
        public int GetActiveAgentsCount()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {

                // ✅ No "Status" column in Agents table, so just count all agents
                string sql = "SELECT COUNT(*) FROM Agents";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }


        // 💸 Get total pending commissions (not yet released)
        // using Real_Estate_Agencies.Data; // ensure correct using
        public decimal GetPendingCommissionsTotal()
        {
            decimal total = 0m;
            try
            {
                var agentRepo = new AgentRepository();
                var agents = agentRepo.GetAllAgents(); // returns list of agents with AgentId

                foreach (var a in agents)
                {
                    // GetAgentBalanceThisMonth already returns Math.Max(balance,0)
                    total += agentRepo.GetAgentBalanceThisMonth(a.AgentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating pending commissions: {ex.Message}", "DB Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return total;
        }








        private string ConvertImageToBase64(byte[] imageBytes)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}