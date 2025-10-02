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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Simple validation
            if (string.IsNullOrEmpty(TxtSaleId.Text) ||
                string.IsNullOrEmpty(TxtClientId.Text) ||
                string.IsNullOrEmpty(TxtPropertyId.Text) ||
                string.IsNullOrEmpty(TxtAgentId.Text) ||
                SaleDatePicker.SelectedDate == null ||
                PaymentModeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }   

            // Create new Sale object
            NewSale = new Sale
            {
                SaleId = int.Parse(TxtSaleId.Text),
                ClientId = int.Parse(TxtClientId.Text),
                PropertyId = int.Parse(TxtPropertyId.Text),
                AgentId = int.Parse(TxtAgentId.Text),
                SaleDate = SaleDatePicker.SelectedDate.Value,
                PaymentMode = (PaymentModeComboBox.SelectedItem as ComboBoxItem).Content.ToString()
            };

            this.DialogResult = true;
            this.Close();
        }
    }
}
