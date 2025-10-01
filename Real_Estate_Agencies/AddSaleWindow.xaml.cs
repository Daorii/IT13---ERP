using System;
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
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSaleId.Text) ||
                string.IsNullOrWhiteSpace(TxtClientId.Text) ||
                string.IsNullOrWhiteSpace(TxtPropertyId.Text) ||
                string.IsNullOrWhiteSpace(TxtAgentId.Text) ||
                SaleDatePicker.SelectedDate == null ||
                PaymentModeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewSale = new Sale
            {
                SaleId = TxtSaleId.Text.Trim(),
                ClientId = TxtClientId.Text.Trim(),
                PropertyId = TxtPropertyId.Text.Trim(),
                AgentId = TxtAgentId.Text.Trim(),
                SaleDate = SaleDatePicker.SelectedDate.Value,
                PaymentMode = ((ComboBoxItem)PaymentModeComboBox.SelectedItem).Content.ToString()
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
