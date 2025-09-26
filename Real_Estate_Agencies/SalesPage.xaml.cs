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
            // Sample data for design purposes
            allSales = new List<Sale>
            {
                new Sale { SaleId = "S001", Customer = "Maria Santos", Agent = "Agent A001", Date = new DateTime(2023, 1, 15), Amount = "₱2,500,000" },
                new Sale { SaleId = "S002", Customer = "John Reyes", Agent = "Agent A002", Date = new DateTime(2023, 4, 20), Amount = "₱3,100,000" },
                new Sale { SaleId = "S003", Customer = "Ana Cruz", Agent = "Agent A001", Date = new DateTime(2023, 7, 2), Amount = "₱4,000,000" },
                new Sale { SaleId = "S004", Customer = "Pedro Lopez", Agent = "Agent A003", Date = new DateTime(2023, 9, 12), Amount = "₱5,200,000" },
                new Sale { SaleId = "S005", Customer = "Carmen Dela Cruz", Agent = "Agent A002", Date = new DateTime(2023, 11, 5), Amount = "₱2,800,000" },
                new Sale { SaleId = "S006", Customer = "Roberto Garcia", Agent = "Agent A001", Date = new DateTime(2024, 2, 18), Amount = "₱6,500,000" },
                new Sale { SaleId = "S007", Customer = "Isabella Fernandez", Agent = "Agent A003", Date = new DateTime(2024, 5, 25), Amount = "₱3,750,000" }
            };

            SalesDataGrid.ItemsSource = allSales;
        }

        // Filter button click event
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        // Date picker change events
        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ToDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Sort combobox change event
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Clear filters button
        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;

            var sortComboBox = FindName("SortComboBox") as ComboBox;
            if (sortComboBox != null)
                sortComboBox.SelectedIndex = 0;

            SalesDataGrid.ItemsSource = allSales;
        }

        // Main filter logic
        private void ApplyFilter()
        {
            if (allSales == null) return;

            var filtered = allSales.AsEnumerable();

            // Date filtering
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (fromDate.HasValue || toDate.HasValue)
            {
                filtered = filtered.Where(s =>
                {
                    bool afterFrom = !fromDate.HasValue || s.Date >= fromDate.Value;
                    bool beforeTo = !toDate.HasValue || s.Date <= toDate.Value;
                    return afterFrom && beforeTo;
                });
            }

            // Sorting
            var sortComboBox = FindName("SortComboBox") as ComboBox;
            if (sortComboBox != null)
            {
                switch (sortComboBox.SelectedIndex)
                {
                    case 0: // Sort by Date
                        filtered = filtered.OrderByDescending(s => s.Date);
                        break;
                    case 1: // Sort by Amount
                        filtered = filtered.OrderByDescending(s => ParseAmount(s.Amount));
                        break;
                    case 2: // Sort by Customer
                        filtered = filtered.OrderBy(s => s.Customer);
                        break;
                }
            }

            SalesDataGrid.ItemsSource = filtered.ToList();
        }

        // Helper method to parse amount for sorting
        private decimal ParseAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount)) return 0;

            string cleanAmount = amount.Replace("₱", "").Replace(",", "").Trim();
            if (decimal.TryParse(cleanAmount, out decimal result))
                return result;

            return 0;
        }
    }

    public class Sale
    {
        public string SaleId { get; set; }
        public string Customer { get; set; }
        public string Agent { get; set; }
        public DateTime Date { get; set; }
        public string Amount { get; set; }
    }
}