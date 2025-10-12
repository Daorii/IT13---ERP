using System;
using System.Windows;
using Real_Estate_Agencies.Model;
using Microsoft.Data.SqlClient;
namespace Real_Estate_Agencies
{
    public partial class AddPaymentWindow : Window
    {
        public Payment NewPayment { get; private set; }
        private int SaleId;

        // ✅ Add these two fields
        private decimal UnitPrice;
        private string PaymentMode;

        public AddPaymentWindow(int saleId)
        {
            InitializeComponent();
            SaleId = saleId;
            LoadSaleDetails();
        }

        private void LoadSaleDetails()
        {
            try
            {
                string connStr = "Data Source=db29561.public.databaseasp.net;User ID=db29561;Password=123456789;Encrypt=False;Trust Server Certificate=True";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                        SELECT p.Price, s.PaymentMode
                        FROM Sales s
                        INNER JOIN Properties p ON s.PropertyID = p.PropertyID
                        WHERE s.SaleID = @SaleID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", SaleId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UnitPrice = reader.GetDecimal(0);
                                PaymentMode = reader.GetString(1);

                                TxtUnitPrice.Text = UnitPrice.ToString("C");
                                if (PaymentMode == "One-time Payment")
                                {
                                    TxtAmount.Text = UnitPrice.ToString("C");
                                }
                                else if (PaymentMode == "Installment")
                                {
                                    decimal monthlyPayment = UnitPrice / 60; // 5 years = 60 months
                                    TxtAmount.Text = monthlyPayment.ToString("C");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Sale details not found for this Sale ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sale details: {ex.Message}");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentDatePicker.SelectedDate == null || PaymentMethodComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠ Please fill in all required fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal amount = 0;
            if (PaymentMode == "One-time Payment")
                amount = UnitPrice;
            else
                amount = UnitPrice / 60;

            NewPayment = new Payment
            {
                SaleId = SaleId,
                Amount = amount,
                PaymentDate = PaymentDatePicker.SelectedDate.Value,
                PaymentType = ((System.Windows.Controls.ComboBoxItem)PaymentMethodComboBox.SelectedItem).Content.ToString()
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
