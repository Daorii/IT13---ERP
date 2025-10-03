using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AddSaleWindow : Window
    {
        public Sale NewSale { get; private set; }

        public AddSaleWindow()
        {
            InitializeComponent();
            // Populate PaymentMode dropdown
            PaymentModeComboBox.Items.Add("One-time Payment");
            PaymentModeComboBox.Items.Add("Installment");

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtClientId.Text) ||
                string.IsNullOrWhiteSpace(TxtPropertyId.Text) ||
                string.IsNullOrWhiteSpace(TxtAgentId.Text) ||
                !SaleDatePicker.SelectedDate.HasValue ||
                PaymentModeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ✅ Just create Sale, DO NOT insert here
            NewSale = new Sale
            {
                ClientId = int.Parse(TxtClientId.Text),
                PropertyId = int.Parse(TxtPropertyId.Text),
                AgentId = int.Parse(TxtAgentId.Text),
                SaleDate = SaleDatePicker.SelectedDate.Value,
                PaymentMode = PaymentModeComboBox.SelectedItem.ToString()
            };

            DialogResult = true; // close and return data
            Close();
        }

        // Click handler wired in XAML: Click="Add_Click"
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // validate numeric IDs
            if (!int.TryParse(TxtClientId.Text?.Trim(), out int clientId))
            {
                MessageBox.Show("Invalid Client ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtClientId.Focus();
                return;
            }

            if (!int.TryParse(TxtPropertyId.Text?.Trim(), out int propertyId))
            {
                MessageBox.Show("Invalid Property ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPropertyId.Focus();
                return;
            }

            if (!int.TryParse(TxtAgentId.Text?.Trim(), out int agentId))
            {
                MessageBox.Show("Invalid Agent ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtAgentId.Focus();
                return;
            }

            if (!SaleDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please pick a sale date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string paymentMode = (PaymentModeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();


            if (string.IsNullOrWhiteSpace(paymentMode))
            {
                MessageBox.Show("Please select a payment mode.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create the Sale object WITHOUT SaleId — DB will generate it
            NewSale = new Sale
            {
                ClientId = clientId,
                PropertyId = propertyId,
                AgentId = agentId,
                SaleDate = SaleDatePicker.SelectedDate.Value, // ensure SaleDate is DateTime in model
                PaymentMode = paymentMode
            };

            DialogResult = true;
            Close();
        }

        // Click handler wired in XAML: Click="Cancel_Click"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}