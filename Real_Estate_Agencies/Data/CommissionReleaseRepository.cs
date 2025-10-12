using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class CommissionReleaseRepository
    {

        public void Add(int agentId, decimal amount)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {
                    string sql = @"
                INSERT INTO CommissionReleases (AgentID, ReleaseDate, AmountReleased)
                VALUES (@AgentID, GETDATE(), @AmountReleased)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", agentId);
                        cmd.Parameters.AddWithValue("@AmountReleased", amount);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (Add Commission Release): {ex.Message}",
                                "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // ✅ Add this new method
        public List<CommissionReleaseModel> GetAll()
        {
            var releases = new List<CommissionReleaseModel>();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {
                    string sql = @"
                SELECT 
                    cr.ReleaseID,
                    cr.AgentID,
                    a.FirstName + ' ' + a.LastName AS AgentName,
                    cr.ReleaseDate,
                       cr.AmountReleased  -- ✅ Make sure this column exists in your DB
                FROM CommissionReleases cr
                INNER JOIN Agents a ON cr.AgentID = a.AgentID
                ORDER BY cr.ReleaseDate DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            releases.Add(new CommissionReleaseModel
                            {
                                ReleaseID = reader.GetInt32(0),
                                AgentID = reader.GetInt32(1),
                                AgentName = reader.GetString(2),
                                ReleaseDate = reader.GetDateTime(3),
                                ReleasedAmount = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllCommissionReleases): {ex.Message}", "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return releases;
        }

    }
}
