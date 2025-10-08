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
                                HireDate = reader.IsDBNull(3) ? string.Empty : reader.GetDateTime(3).ToString("yyyy-MM-dd"),
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

                        // Explicitly type the ProfileImage parameter as varbinary(max)
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
    }
}
