using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class AgentRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        // ✅ Fetch all agents
        public List<Agent> GetAllAgents()
        {
            var agents = new List<Agent>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT AgentID, FirstName, LastName, HireDate, ContactInfo, ProfileImage FROM Agents";



                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int counter = 1;
                        while (reader.Read())
                        {
                            agents.Add(new Agent
                            {
                                No = counter++,
                                AgentId = reader.GetInt32(0),
                                FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                HireDate = reader.IsDBNull(3)
                            ? string.Empty
                            : (reader[3] is DateTime dt
                                ? dt.ToString("yyyy-MM-dd")
                                : DateTime.TryParse(reader.GetString(3), out var parsedDate)
                                    ? parsedDate.ToString("yyyy-MM-dd")
                                    : string.Empty),
                                ContactInfo = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                ProfileImage = reader.IsDBNull(5) ? null : (byte[])reader[5]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllAgents): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return agents;
        }

        // ✅ Add new agent
        public void AddAgent(Agent agent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Agents (FirstName, LastName, HireDate, ContactInfo, ProfileImage)
                                   VALUES (@FirstName, @LastName, @HireDate, @ContactInfo, @ProfileImage);
                                   SELECT CAST(scope_identity() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", agent.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", agent.LastName);

                        DateTime hireDate = DateTime.TryParse(agent.HireDate, out var d) ? d : DateTime.Now;
                        cmd.Parameters.AddWithValue("@HireDate", hireDate);

                        cmd.Parameters.AddWithValue("@ContactInfo", agent.ContactInfo ?? string.Empty);
                        cmd.Parameters.Add("@ProfileImage", System.Data.SqlDbType.VarBinary, -1).Value =
                            agent.ProfileImage ?? (object)DBNull.Value;

                        agent.AgentId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (AddAgent): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Update existing agent
        public void UpdateAgent(Agent agent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE Agents
                                   SET FirstName=@FirstName, LastName=@LastName, HireDate=@HireDate,
                                       ContactInfo=@ContactInfo, ProfileImage=@ProfileImage
                                   WHERE AgentID=@AgentId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentId", agent.AgentId);
                        cmd.Parameters.AddWithValue("@FirstName", agent.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", agent.LastName);

                        DateTime hireDate = DateTime.TryParse(agent.HireDate, out var d) ? d : DateTime.Now;
                        cmd.Parameters.AddWithValue("@HireDate", hireDate);

                        cmd.Parameters.AddWithValue("@ContactInfo", agent.ContactInfo ?? string.Empty);
                        cmd.Parameters.Add("@ProfileImage", System.Data.SqlDbType.VarBinary, -1).Value =
                            agent.ProfileImage ?? (object)DBNull.Value;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (UpdateAgent): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Delete agent
        public void DeleteAgent(int agentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Agents WHERE AgentID = @AgentId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentId", agentId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (DeleteAgent): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Get total commission + incentives (this month)
        // Get total commission + incentives (this month)
        public decimal GetAgentBalanceThisMonth(int agentId)
        {
            decimal totalCommissions = 0;
            decimal totalIncentives = 0;
            decimal totalReleases = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // --- Commissions ---
                    string sqlCommissions = @"
                SELECT ISNULL(SUM(c.CommissionAmount), 0)
                FROM Commissions c
                JOIN Payments p ON c.PaymentID = p.PaymentID
                JOIN Sales s ON p.SaleID = s.SaleID
                WHERE s.AgentID = @AgentID
                  AND MONTH(c.ReleaseDate) = MONTH(GETDATE())
                  AND YEAR(c.ReleaseDate) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sqlCommissions, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        totalCommissions = Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // --- Incentives ---
                    string sqlIncentives = @"
                SELECT ISNULL(SUM(i.Amount), 0)
                FROM Incentives i
                WHERE i.AgentID = @AgentID
                  AND MONTH(i.ReleaseDate) = MONTH(GETDATE())
                  AND YEAR(i.ReleaseDate) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sqlIncentives, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        totalIncentives = Convert.ToDecimal(cmd.ExecuteScalar());
                    }

                    // --- Releases ---
                    string sqlReleases = @"
                SELECT ISNULL(SUM(r.AmountReleased), 0)
                FROM CommissionReleases r
                WHERE r.AgentID = @AgentID
                  AND MONTH(r.ReleaseDate) = MONTH(GETDATE())
                  AND YEAR(r.ReleaseDate) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sqlReleases, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        totalReleases = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAgentBalanceThisMonth): {ex.Message}",
                                "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // ✅ Balance = commissions + incentives - released
            decimal balance = totalCommissions + totalIncentives - totalReleases;
            return Math.Max(balance, 0);
        }



        // ✅ Get number of sales for this month
        public int GetAgentSalesCountThisMonth(int agentId)
        {
            int count = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                        SELECT COUNT(*) 
                        FROM Sales 
                        WHERE AgentID = @AgentID 
                          AND MONTH(SaleDate) = MONTH(GETDATE())
                          AND YEAR(SaleDate) = YEAR(GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        count = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAgentSalesCountThisMonth): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return count;
        }

        // ✅ Reset agent balance after "release" (optional log)
        public void ResetAgentBalance(int agentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Log the release if you have a table for it
                    string sql = @"
                        IF OBJECT_ID('CommissionReleases', 'U') IS NOT NULL
                        INSERT INTO CommissionReleases (AgentID, ReleaseDate)
                        VALUES (@AgentID, GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (ResetAgentBalance): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
