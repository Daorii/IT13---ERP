using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class SalesPage : Page
    {
        public SalesPage()
        {
            InitializeComponent();
            LoadSales();
        }

        private void LoadSales()
        {
            // Dummy sales data for testing
            SalesGrid.ItemsSource = new List<SaleModel>
            {
                new SaleModel { SaleId = "S001", ClientId = "C001", PropertyId = "P001", SaleDate = "2025-09-01", PaymentOption = "Cash", TotalPrice = "$200,000" },
                new SaleModel { SaleId = "S002", ClientId = "C002", PropertyId = "P002", SaleDate = "2025-09-10", PaymentOption = "Installment", TotalPrice = "$150,000" }
            };
        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            AddSale addSaleWindow = new AddSale();
            if (addSaleWindow.ShowDialog() == true)
            {
                var newSale = addSaleWindow.NewSale;
                var sales = (List<SaleModel>)SalesGrid.ItemsSource;
                sales.Add(newSale);

                SalesGrid.ItemsSource = null;
                SalesGrid.ItemsSource = sales;
            }
        }
    }

    public class SaleModel
    {
        public string SaleId { get; set; }
        public string ClientId { get; set; }
        public string PropertyId { get; set; }
        public string SaleDate { get; set; }
        public string PaymentOption { get; set; }
        public string TotalPrice { get; set; }
    }
}
