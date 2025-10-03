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


        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Implement pagination logic if needed
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Implement pagination logic if needed
        }
    }
}
