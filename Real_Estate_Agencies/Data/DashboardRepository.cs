using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class DashboardRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<PropertyModel> GetTopSoldProperties(int topCount)
        {
            var list = new List<PropertyModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = $@"
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
                MessageBox.Show($"Database error (GetTopSoldProperties): {ex.Message}",
                                "DB Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        public List<AgentPerformanceModel> GetTopAgents(int topCount)
        {
            var list = new List<AgentPerformanceModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $@"
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




        private string ConvertImageToBase64(byte[] imageBytes)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}
