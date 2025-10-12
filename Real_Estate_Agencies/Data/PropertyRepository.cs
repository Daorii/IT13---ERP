using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Data;

namespace Real_Estate_Agencies.Repositories
{
    public class PropertyRepository
    {

        public List<PropertyModel> GetAll()
        {
            var properties = new List<PropertyModel>();

            try
            {
                using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
                {
                    string sql = "SELECT PropertyId, Name, Location, Type, Category, Price, Status, Image FROM Properties";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                properties.Add(new PropertyModel
                                {
                                    PropertyId = reader.GetInt32(0),
                                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Location = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    PropertyType = reader.IsDBNull(3) ? null : reader.GetString(3), // map "Type" to PropertyType
                                    Category = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Price = reader.IsDBNull(5) ? 0 : Convert.ToDouble(reader.GetValue(5)),
                                    Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Image = reader.IsDBNull(7) ? null : (byte[])reader[7]
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching properties from database: " + ex.Message);
            }

            return properties;
        }

        public void Add(PropertyModel property)
        {
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"INSERT INTO Properties (Name, Location, Type, Category, Price, Status, Image)
                       VALUES (@Name, @Location, @Type, @Category, @Price, @Status, @Image)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", property.Name);
                    cmd.Parameters.AddWithValue("@Location", property.Location);
                    cmd.Parameters.AddWithValue("@Type", property.PropertyType); // map PropertyType to Type column
                    cmd.Parameters.AddWithValue("@Category", property.Category);
                    cmd.Parameters.AddWithValue("@Price", property.Price);
                    cmd.Parameters.AddWithValue("@Status", property.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Image", (object)property.Image ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Update(PropertyModel property)
        {
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = @"UPDATE Properties SET
                       Name=@Name, 
                       Location=@Location,
                       Type=@Type,
                       Category=@Category,
                       Price=@Price,
                       Status=@Status,
                       Image=@Image
                        WHERE PropertyId=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", property.PropertyId);


                    cmd.Parameters.AddWithValue("@Name", property.Name);
                    cmd.Parameters.AddWithValue("@Location", property.Location);
                    cmd.Parameters.AddWithValue("@Type", property.PropertyType); // map PropertyType to Type
                    cmd.Parameters.AddWithValue("@Category", property.Category);
                    cmd.Parameters.AddWithValue("@Price", property.Price);
                    cmd.Parameters.AddWithValue("@Status", property.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Image", (object)property.Image ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = "DELETE FROM Properties WHERE PropertyId=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
