using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class AddSale : Window
    {
        public SaleModel NewSale { get; private set; }

        public AddSale()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            NewSale = new SaleModel
            {
                SaleId = SaleIdInput.Text,
                ClientId = ClientIdInput.Text,
                PropertyId = PropertyIdInput.Text,
                SaleDate = SaleDateInput.SelectedDate?.ToString("yyyy-MM-dd"),
                PaymentOption = (PaymentOptionInput.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString(),
                TotalPrice = TotalPriceInput.Text
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
