using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PaymentPage : Page
    {
        private ObservableCollection<SalePayment> payments;
        private int SaleId;

        public PaymentPage(int saleId)
        {
            InitializeComponent();
            SaleId = saleId;
            SaleIdTextBlock.Text = $"Payments for Sale ID: {SaleId}";
            LoadPayments();
        }

        private void LoadPayments()
        {
            payments = new ObservableCollection<SalePayment>
            {
                new SalePayment { PaymentId="P001", SaleId=1, PaymentType="Cash", PaymentAmount=5000, PaymentDate=DateTime.Now },
                new SalePayment { PaymentId="P002", SaleId=1, PaymentType="Cash", PaymentAmount=2000, PaymentDate=DateTime.Now },
                new SalePayment { PaymentId="P003", SaleId=2, PaymentType="Bank Loan", PaymentAmount=10000, PaymentDate=DateTime.Now },
            };

            PaymentsDataGrid.ItemsSource = new ObservableCollection<SalePayment>(payments.Where(p => p.SaleId == SaleId));
        }

        // Add Payment button click
        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentWindow addPaymentWindow = new AddPaymentWindow();
            if (addPaymentWindow.ShowDialog() == true)
            {
                SalePayment newPayment = new SalePayment
                {
                    PaymentId = addPaymentWindow.NewPayment.PaymentId,
                    SaleId = SaleId,
                    PaymentType = addPaymentWindow.NewPayment.PaymentMethod,
                    PaymentAmount = addPaymentWindow.NewPayment.Amount,
                    PaymentDate = addPaymentWindow.NewPayment.PaymentDate
                };

                payments.Add(newPayment);
                PaymentsDataGrid.ItemsSource = new ObservableCollection<SalePayment>(payments.Where(p => p.SaleId == SaleId));
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

    public class SalePayment
    {
        public string PaymentId { get; set; }
        public int SaleId { get; set; }
        public string PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
