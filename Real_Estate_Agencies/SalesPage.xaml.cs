using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class SalesPage : Page
    {
        private List<Sale> allSales;

        public SalesPage()
        {
            InitializeComponent();
            LoadSales();
        }

        private void LoadSales()
        {
            allSales = new List<Sale>
            {
                new Sale { SaleId = "S001", ClientId = "C001", PropertyId = "P001", AgentId = "A001", SaleDate = new DateTime(2023,1,15), PaymentMode="One-time Payment" },
                new Sale { SaleId = "S002", ClientId = "C002", PropertyId = "P002", AgentId = "A002", SaleDate = new DateTime(2023,4,20), PaymentMode="Installment" },
                new Sale { SaleId = "S003", ClientId = "C003", PropertyId = "P003", AgentId = "A001", SaleDate = new DateTime(2023,7,2), PaymentMode="One-time Payment" },
                new Sale { SaleId = "S004", ClientId = "C004", PropertyId = "P004", AgentId = "A003", SaleDate = new DateTime(2023,9,12), PaymentMode="Installment" }
            };

            RefreshDataGrid(allSales);
        }

        private void RefreshDataGrid(IEnumerable<Sale> sales)
        {
            if (SalesDataGrid == null) return;

            var numberedList = sales.Select((s, index) => new
            {
                Number = (index + 1).ToString(),
                s.SaleId,
                s.ClientId,
                s.PropertyId,
                s.AgentId,
                SaleDate = s.SaleDate.ToString("yyyy-MM-dd"),
                s.PaymentMode
            }).ToList();

            SalesDataGrid.ItemsSource = numberedList;
        }

        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void ToDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            SortComboBox.SelectedIndex = 0;
            RefreshDataGrid(allSales);
        }

        private void ApplyFilter()
        {
            if (allSales == null) return;

            var filtered = allSales.AsEnumerable();

            DateTime? from = FromDatePicker.SelectedDate;
            DateTime? to = ToDatePicker.SelectedDate;

            if (from.HasValue && to.HasValue && from > to)
            {
                MessageBox.Show("⚠ 'From' date cannot be later than 'To' date.", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (from.HasValue || to.HasValue)
            {
                filtered = filtered.Where(s =>
                {
                    bool afterFrom = !from.HasValue || s.SaleDate >= from.Value;
                    bool beforeTo = !to.HasValue || s.SaleDate <= to.Value;
                    return afterFrom && beforeTo;
                });
            }

            switch (SortComboBox.SelectedIndex)
            {
                case 0: filtered = filtered.OrderByDescending(s => s.SaleDate); break;
                case 1: filtered = filtered.OrderBy(s => ExtractNumeric(s.ClientId)); break;
            }

            RefreshDataGrid(filtered);
        }

        private int ExtractNumeric(string id)
        {
            if (string.IsNullOrEmpty(id)) return 0;
            string numPart = new string(id.Where(char.IsDigit).ToArray());
            return int.TryParse(numPart, out int result) ? result : 0;
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddSaleWindow();
            if (addWindow.ShowDialog() == true && addWindow.NewSale != null)
            {
                allSales.Add(addWindow.NewSale);
                RefreshDataGrid(allSales);
            }
        }

        // ✅ New handler for the "View" button in Sales list
        private void ViewSale_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button?.DataContext == null) return;

            // Get SaleId from DataContext
            var saleIdProp = button.DataContext.GetType().GetProperty("SaleId");
            string saleId = saleIdProp?.GetValue(button.DataContext)?.ToString();

            if (string.IsNullOrEmpty(saleId)) return;

            // Find the matching sale
            var selectedSale = allSales.FirstOrDefault(s => s.SaleId == saleId);
            if (selectedSale != null)
            {
                var detailWindow = new SaleDetailWindow(selectedSale);
                detailWindow.ShowDialog();
            }
        }
    }

    public class Sale
    {
        public string SaleId { get; set; }
        public string ClientId { get; set; }
        public string PropertyId { get; set; }
        public string AgentId { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMode { get; set; }
    }
}
