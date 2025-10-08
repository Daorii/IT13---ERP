using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies.Data
{
    public class ClientRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT ClientId, FirstName, LastName, ContactInfo, Address FROM Clients";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                ClientId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                ContactInfo = reader.GetString(3),
                                Address = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllClients): {ex.Message}", "DB Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return clients;
        }

        public void AddClient(Client client)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Clients (FirstName, LastName, ContactInfo, Address)
               VALUES (@FirstName, @LastName, @ContactInfo, @Address);
               SELECT CAST(SCOPE_IDENTITY() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", client.LastName);
                        cmd.Parameters.AddWithValue("@ContactInfo", client.ContactInfo);
                        cmd.Parameters.AddWithValue("@Address", client.Address);

                        client.ClientId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (AddClient): {ex.Message}", "DB Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateClient(Client client)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE Clients
               SET FirstName=@FirstName, LastName=@LastName,
                   ContactInfo=@ContactInfo, Address=@Address
               WHERE ClientId=@ClientId";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientId", client.ClientId);
                        cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", client.LastName);
                        cmd.Parameters.AddWithValue("@ContactInfo", client.ContactInfo);
                        cmd.Parameters.AddWithValue("@Address", client.Address);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (UpdateClient): {ex.Message}", "DB Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteClient(int clientId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Clients WHERE ClientId=@ClientId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientId", clientId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (DeleteClient): {ex.Message}", "DB Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
