using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class SaleDetailWindow : Window
    {
        public SaleDetailWindow(Sale sale)
        {
            InitializeComponent();
            DataContext = sale; // bind sale data to the UI
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            // Open Add Payment dialog
            AddPaymentWindow paymentWindow = new AddPaymentWindow();
            paymentWindow.Owner = this;
            if (paymentWindow.ShowDialog() == true)
            {
                MessageBox.Show("Payment added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
