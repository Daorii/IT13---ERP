using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class SalesRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Sale> GetAllSales()
        {
            var sales = new List<Sale>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                SELECT 
                    s.SaleID, s.ClientID, s.PropertyID, s.PropertyName, s.AgentID, s.SaleDate, s.PaymentMode,
                    c.FirstName + ' ' + c.LastName AS ClientName,
                    a.FirstName + ' ' + a.LastName AS AgentName,
                    p.Type AS PropertyType
                FROM Sales s
                LEFT JOIN Clients c ON s.ClientID = c.ClientID
                LEFT JOIN Agents a ON s.AgentID = a.AgentID
                LEFT JOIN Properties p ON s.PropertyID = p.PropertyId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(new Sale
                            {
                                SaleId = reader.GetInt32(0),
                                ClientId = reader.GetInt32(1),
                                PropertyId = reader.GetInt32(2),
                                PropertyName = reader.GetString(3),
                                AgentId = reader.GetInt32(4),
                                SaleDate = reader.GetDateTime(5),
                                PaymentMode = reader.GetString(6),
                                ClientName = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                AgentName = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                PropertyType = reader.IsDBNull(9) ? "" : reader.GetString(9)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllSales): {ex.Message}", "DB Error");
            }
            return sales;
        }



        public void AddSale(Sale sale)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                INSERT INTO Sales (ClientID, PropertyID, AgentID, SaleDate, PaymentMode, PropertyName)
                VALUES (@ClientID, @PropertyID, @AgentID, @SaleDate, @PaymentMode, @PropertyName);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientID", sale.ClientId);
                        cmd.Parameters.AddWithValue("@PropertyID", sale.PropertyId);
                        cmd.Parameters.AddWithValue("@AgentID", sale.AgentId);
                        cmd.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                        cmd.Parameters.AddWithValue("@PaymentMode", sale.PaymentMode ?? "");
                        cmd.Parameters.AddWithValue("@PropertyName", sale.PropertyName ?? "");

                        sale.SaleId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (AddSale): {ex.Message}", "DB Error");
            }
        }


    }
}
