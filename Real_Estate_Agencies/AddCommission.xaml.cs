using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies.Models;

namespace Real_Estate_Agencies
{
    public partial class AddCommission : Window
    {
        public Commission NewCommission { get; set; }

        public AddCommission()
        {
            InitializeComponent();
            // Remove these lines since the textboxes no longer exist
            // TxtCommissionId.Text = "(Auto)";
            // TxtPaymentId.Text = "(Auto)";
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TxtReleaseDate.IsReadOnly = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                decimal.TryParse(TxtCommissionRate.Text, out decimal rate);
                decimal.TryParse(TxtCommissionAmount.Text, out decimal amount);
                DateTime.TryParse(TxtReleaseDate.Text, out DateTime date);

                // Use the Models.Commission (int/decimal properties)
                NewCommission = new Commission
                {
                    // CommissionId and PaymentId will be auto-generated in the conversion method
                    SalesId = int.Parse(TxtSalesId.Text),
                    AgentId = int.Parse(TxtAgentId.Text),
                    CommissionRate = rate,
                    CommissionAmount = amount,
                    Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Pending",
                    ReleaseDate = date
                };

                MessageBox.Show("Commission added successfully!", "Success");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TxtSalesId.Text) || !int.TryParse(TxtSalesId.Text, out _))
            {
                MessageBox.Show("Sales ID must be a valid number.", "Validation Error");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtAgentId.Text) || !int.TryParse(TxtAgentId.Text, out _))
            {
                MessageBox.Show("Agent ID must be a valid number.", "Validation Error");
                return false;
            }

            if (!decimal.TryParse(TxtCommissionRate.Text, out _))
            {
                MessageBox.Show("Please enter a valid commission rate.", "Validation Error");
                return false;
            }

            if (!decimal.TryParse(TxtCommissionAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid commission amount.", "Validation Error");
                return false;
            }

            return true;
        }
    }
}