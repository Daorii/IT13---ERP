using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PaymentPage : Page
    {
        private ObservableCollection<Payment> payments;
        private readonly PaymentsRepository repo;
        private int SaleId;

        public PaymentPage(int saleId)
        {
            InitializeComponent();
            SaleId = saleId;
            repo = new PaymentsRepository();
            SaleIdTextBlock.Text = $"Payments for Sale ID: {SaleId}";
            LoadPayments();
        }

        private void LoadPayments()
        {
            var paymentList = repo.GetPaymentsBySaleId(SaleId);
            payments = new ObservableCollection<Payment>(paymentList);
            PaymentsDataGrid.ItemsSource = payments;

            bool isFullyPaid = IsSaleFullyPaid();

            if (isFullyPaid)
            {
                MessageBox.Show("✅ This sale is already fully paid. No further payments can be added.",
                    "Payment Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                // Disable the Add Payment button
                AddPaymentButton.IsEnabled = false;
                AddPaymentButton.Opacity = 0.5;
            }
            else
            {
                AddPaymentButton.IsEnabled = true;
                AddPaymentButton.Opacity = 1;
            }


        }


        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            var addPaymentWindow = new AddPaymentWindow(SaleId); // pass SaleId
            if (addPaymentWindow.ShowDialog() == true)
            {
                var newPayment = addPaymentWindow.NewPayment;
                repo.AddPayment(newPayment);
                payments.Add(newPayment);
            }
        }

        private bool IsSaleFullyPaid()
        {
            try
            {
                string connStr = "Data Source=db29561.public.databaseasp.net;User ID=db29561;Password=123456789;Encrypt=False;Trust Server Certificate=True";

                using (var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                SELECT 
                    p.Price AS PropertyPrice,
                    ISNULL(SUM(py.Amount), 0) AS TotalPaid
                FROM Sales s
                INNER JOIN Properties p ON s.PropertyID = p.PropertyID
                LEFT JOIN Payments py ON s.SaleID = py.SaleID
                WHERE s.SaleID = @SaleID
                GROUP BY p.Price";

                    using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SaleID", SaleId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal propertyPrice = reader.GetDecimal(0);
                                decimal totalPaid = reader.GetDecimal(1);

                                return totalPaid >= propertyPrice;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking payment status: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }



        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Implement pagination logic if needed
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Implement pagination logic if needed
        }
        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

    }
}
