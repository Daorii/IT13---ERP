using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Real_Estate_Agencies
{
    public partial class SalesPage : Page
    {
        private readonly SalesRepository _repo;
        private List<Sale> AllSales;
        private List<Sale> FilteredSales;
        private int CurrentPage = 1;
        private int PageSize = 10;

        public SalesPage()
        {
            InitializeComponent();
            _repo = new SalesRepository();
            LoadSales();
            ApplyFilter();
        }

        private void LoadSales()
        {
            AllSales = _repo.GetAllSales();
        }

        private void ApplyFilter()
        {
            DateTime? from = FromDatePicker?.SelectedDate;
            DateTime? to = ToDatePicker?.SelectedDate;

            FilteredSales = AllSales?
     .Where(s => (!from.HasValue || s.SaleDate >= from.Value) &&
                 (!to.HasValue || s.SaleDate <= to.Value))
     .ToList() ?? new List<Sale>();

            if (SortComboBox?.SelectedIndex == 0)
                FilteredSales = FilteredSales.OrderBy(s => s.SaleDate).ToList();
            else if (SortComboBox?.SelectedIndex == 1)
                FilteredSales = FilteredSales.OrderByDescending(s => s.SaleDate).ToList();


            CurrentPage = 1;
            RefreshPage();
        }

        private void RefreshPage()
        {
            if (FilteredSales == null || SalesDataGrid == null) return;

            var pageData = FilteredSales.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            SalesDataGrid.ItemsSource = pageData;

            PageInfoTextBlock.Text = $"Showing {((CurrentPage - 1) * PageSize + 1)}-{Math.Min(CurrentPage * PageSize, FilteredSales.Count)} of {FilteredSales.Count} sales";
        }

        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                RefreshPage();
            }
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < Math.Ceiling((double)FilteredSales.Count / PageSize))
            {
                CurrentPage++;
                RefreshPage();
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ToDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddSaleWindow();
            if (addWindow.ShowDialog() == true)
            {
                var newSale = addWindow.NewSale;

                // ✅ Only one insert happens here
                _repo.AddSale(newSale);

                AllSales.Add(newSale);
                ApplyFilter();
            }
        }

        private void SaleRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Sale sale)
            {
                var paymentPage = new PaymentPage(sale.SaleId);
                NavigationService?.Navigate(paymentPage);
            }
        }
    }
}

