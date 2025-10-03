using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
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
                    string sql = "SELECT SaleID, ClientID, PropertyID, AgentID, SaleDate, PaymentMode FROM Sales";

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
                                AgentId = reader.GetInt32(3),
                                SaleDate = reader.GetDateTime(4),   // ✅ keep as DateTime
                                PaymentMode = reader.GetString(5)
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
                    string sql = @"INSERT INTO Sales (ClientID, PropertyID, AgentID, SaleDate, PaymentMode)
                                   VALUES (@ClientID, @PropertyID, @AgentID, @SaleDate, @PaymentMode);
                                   SELECT CAST(scope_identity() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientID", sale.ClientId);
                        cmd.Parameters.AddWithValue("@PropertyID", sale.PropertyId);
                        cmd.Parameters.AddWithValue("@AgentID", sale.AgentId);
                        cmd.Parameters.AddWithValue("@SaleDate", sale.SaleDate); // ✅ directly DateTime
                        cmd.Parameters.AddWithValue("@PaymentMode", sale.PaymentMode);

                        sale.SaleId = (int)cmd.ExecuteScalar(); // return new SaleID
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
