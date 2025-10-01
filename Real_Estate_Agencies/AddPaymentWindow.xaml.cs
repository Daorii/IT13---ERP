using System;
using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class AddPaymentWindow : Window
    {
        public Payment NewPayment { get; private set; }

        public AddPaymentWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPaymentId.Text) ||
                string.IsNullOrWhiteSpace(TxtAmount.Text) ||
                PaymentDatePicker.SelectedDate == null ||
                PaymentMethodComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠ Please fill in all fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtAmount.Text, out decimal amount))
            {
                MessageBox.Show("⚠ Amount must be a valid number.", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewPayment = new Payment
            {
                PaymentId = TxtPaymentId.Text.Trim(),
                Amount = amount,
                PaymentDate = PaymentDatePicker.SelectedDate.Value,
                PaymentMethod = ((System.Windows.Controls.ComboBoxItem)PaymentMethodComboBox.SelectedItem).Content.ToString()
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

    public class Payment
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}
