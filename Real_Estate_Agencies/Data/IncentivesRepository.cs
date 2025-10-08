using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class IncentivesRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Incentive> GetAllIncentives()
        {
            var incentives = new List<Incentive>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
    SELECT i.IncentiveID, i.AgentID, a.FirstName + ' ' + a.LastName AS AgentName,
           i.IncentiveType, i.Amount, i.ReleaseDate
    FROM Incentives i
    INNER JOIN Agents a ON i.AgentID = a.AgentID";


                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            incentives.Add(new Incentive
                            {
                                IncentiveId = reader.GetInt32(0),
                                AgentId = reader.GetInt32(1),
                                AgentName = reader.GetString(2),
                                IncentiveType = reader.GetString(3),
                                Amount = reader.GetDecimal(4),
                                ReleaseDate = reader.GetDateTime(5)
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (GetAllIncentives): {ex.Message}");
            }

            return incentives;
        }

        public void AddIncentive(Incentive incentive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Incentives (AgentID, IncentiveType, Amount, ReleaseDate)
                                   VALUES (@AgentID, @IncentiveType, @Amount, @ReleaseDate);
                                   SELECT CAST(scope_identity() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", incentive.AgentId);
                        cmd.Parameters.AddWithValue("@IncentiveType", incentive.IncentiveType);
                        cmd.Parameters.AddWithValue("@Amount", incentive.Amount);
                        cmd.Parameters.AddWithValue("@ReleaseDate", incentive.ReleaseDate);

                        incentive.IncentiveId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (AddIncentive): {ex.Message}");
            }
        }

        public void UpdateIncentive(Incentive incentive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE Incentives 
                           SET AgentID = @AgentID, 
                               IncentiveType = @IncentiveType, 
                               Amount = @Amount, 
                               ReleaseDate = @ReleaseDate
                           WHERE IncentiveID = @IncentiveID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AgentID", incentive.AgentId);
                        cmd.Parameters.AddWithValue("@IncentiveType", incentive.IncentiveType);
                        cmd.Parameters.AddWithValue("@Amount", incentive.Amount);
                        cmd.Parameters.AddWithValue("@ReleaseDate", incentive.ReleaseDate);
                        cmd.Parameters.AddWithValue("@IncentiveID", incentive.IncentiveId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (UpdateIncentive): {ex.Message}");
            }
        }

        public void DeleteIncentive(int incentiveId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Incentives WHERE IncentiveID = @IncentiveID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IncentiveID", incentiveId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (DeleteIncentive): {ex.Message}");
            }
        }

    }
}

