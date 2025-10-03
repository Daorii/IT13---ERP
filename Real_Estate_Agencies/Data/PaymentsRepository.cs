using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using System.Windows;

namespace Real_Estate_Agencies.Data
{
    public class PaymentsRepository
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=RealEstate;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Payment> GetPaymentsBySaleId(int saleId)
        {
            var payments = new List<Payment>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT PaymentID, SaleID, PaymentType, Amount, PaymentDate FROM Payments WHERE SaleID = @SaleID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", saleId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payments.Add(new Payment
                                {
                                    PaymentId = reader.GetInt32(0),
                                    SaleId = reader.GetInt32(1),
                                    PaymentType = reader.GetString(2),
                                    Amount = reader.GetDecimal(3),
                                    PaymentDate = reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (GetPayments): {ex.Message}");
            }

            return payments;
        }

        public void AddPayment(Payment payment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Payments (SaleID, PaymentType, Amount, PaymentDate)
                                   VALUES (@SaleID, @PaymentType, @Amount, @PaymentDate);
                                   SELECT CAST(scope_identity() AS int);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", payment.SaleId);
                        cmd.Parameters.AddWithValue("@PaymentType", payment.PaymentType);
                        cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                        cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);

                        payment.PaymentId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error (AddPayment): {ex.Message}");
            }
        }
    }
}
