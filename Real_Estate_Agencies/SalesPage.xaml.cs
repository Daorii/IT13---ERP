using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
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
            // Clear previous sales to prevent duplication
            AllSales = new List<Sale>();

            var salesFromDb = _repo.GetAllSales();
            AllSales.AddRange(salesFromDb);

            // map clients, agents, properties
            var clientRepo = new ClientRepository();
            var agentRepo = new AgentRepository();
            var propertyRepo = new PropertyRepository();

            var allClients = clientRepo.GetAllClients();
            var allAgents = agentRepo.GetAllAgents();
            var allProperties = propertyRepo.GetAll();

           
        }


        private void ApplyFilter()
        {
            if (AllSales == null) return;

            DateTime? from = FromDatePicker?.SelectedDate;
            DateTime? to = ToDatePicker?.SelectedDate;

            // If both are null, skip date filtering
            var query = AllSales.AsEnumerable();
            if (from.HasValue)
                query = query.Where(s => s.SaleDate >= from.Value);
            if (to.HasValue)
                query = query.Where(s => s.SaleDate <= to.Value);

            FilteredSales = query.ToList();

            int sortIndex = SortComboBox?.SelectedIndex ?? 0;

            if (sortIndex == 1) // Sort by client
            {
                FilteredSales = FilteredSales
                    .OrderBy(s => string.IsNullOrWhiteSpace(s.ClientName) ? s.ClientId.ToString() : s.ClientName, StringComparer.CurrentCultureIgnoreCase)
                    .ToList();
            }
            else // Sort by date (default)
            {
                // Sort newest first — if same date, latest SaleId first
                FilteredSales = FilteredSales
                    .OrderByDescending(s => s.SaleDate)
                    .ThenByDescending(s => s.SaleId)
                    .ToList();
            }

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

                // Insert into DB
                _repo.AddSale(newSale);

                // Option A: Add just this new sale to the list
                AllSales.Add(newSale);

                // Always keep newest first
                AllSales = AllSales.OrderByDescending(s => s.SaleDate).ToList();

                // Re-apply mapping for names and property type for this new sale
                var clientRepo = new ClientRepository();
                var agentRepo = new AgentRepository();
                var propertyRepo = new PropertyRepository();

                var client = clientRepo.GetAllClients().FirstOrDefault(c => c.ClientId == newSale.ClientId);
                newSale.ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Unknown";

                var agent = agentRepo.GetAllAgents().FirstOrDefault(a => a.AgentId == newSale.AgentId);
                newSale.AgentName = agent != null ? $"{agent.FirstName} {agent.LastName}" : "Unknown";

                var property = propertyRepo.GetAll().FirstOrDefault(p => p.PropertyId == newSale.PropertyId);
                newSale.PropertyType = property != null ? property.PropertyType : "Unknown";

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

