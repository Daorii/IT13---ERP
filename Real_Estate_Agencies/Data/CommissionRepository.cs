using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Models;
using System.Windows;

namespace Real_Estate_Agencies.Repository
{
    public class CommissionRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Commission> GetAllCommissions()
        {
            var commissions = new List<Commission>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT 
                            c.CommissionID,
                            p.PaymentID,
                            s.SaleID,
                            a.AgentID,
                            c.CommissionAmount,
                            c.ReleaseDate
                        FROM Commissions c
                        JOIN Payments p ON c.PaymentID = p.PaymentID
                        JOIN Sales s ON p.SaleID = s.SaleID
                        JOIN Agents a ON s.AgentID = a.AgentID
                        ORDER BY c.ReleaseDate DESC";

                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commissions.Add(new Commission
                            {
                                CommissionId = Convert.ToInt32(reader["CommissionID"]),
                                SalesId = Convert.ToInt32(reader["SaleID"]),
                                AgentId = Convert.ToInt32(reader["AgentID"]),
                                CommissionAmount = (decimal)reader["CommissionAmount"],
                                ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllCommissions): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return commissions;
        }
    }
}
