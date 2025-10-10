using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies.Models;
using Real_Estate_Agencies.Repository;

namespace Real_Estate_Agencies
{
    public partial class CommissionsPage : Page
    {
        private List<Commission> allCommissionsModels; // Raw model
        private Commission commissionToEdit;

        public CommissionsPage()
        {
            InitializeComponent();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            try
            {
                var repo = new CommissionRepository();
                allCommissionsModels = repo.GetAllCommissions() ?? new List<Commission>();
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllCommissions): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDataGrid(List<Commission> commissions = null)
        {
            var displayItems = (commissions ?? allCommissionsModels)
                .Select(c => ConvertToDisplayItem(c))
                .ToList();

            CommissionsDataGrid.ItemsSource = displayItems;
        }

        private CommissionDisplayItem ConvertToDisplayItem(Commission commission)
        {
            return new CommissionDisplayItem
            {
                CommissionID = "C" + commission.CommissionId.ToString("D3"),
                SalesID = "S" + commission.SalesId.ToString("D3"),
                AgentID = "A" + commission.AgentId.ToString("D3"),
                CommissionAmount = commission.CommissionAmount.ToString("C", new CultureInfo("en-PH")),
                ReleaseDate = commission.ReleaseDate.ToString("yyyy-MM-dd")
            };
        }

        // --- EDIT OVERLAY ---
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var displayItem = button.CommandParameter as CommissionDisplayItem;
            if (displayItem == null) return;

            int commissionId = int.Parse(displayItem.CommissionID.Substring(1));
            commissionToEdit = allCommissionsModels.FirstOrDefault(c => c.CommissionId == commissionId);

            if (commissionToEdit != null)
            {
                TxtCommissionId.Text = "C" + commissionToEdit.CommissionId.ToString("D3");
                TxtSalesId.Text = "S" + commissionToEdit.SalesId.ToString("D3");
                TxtAgentId.Text = "A" + commissionToEdit.AgentId.ToString("D3");
                TxtCommissionAmount.Text = commissionToEdit.CommissionAmount.ToString("C", new CultureInfo("en-PH"));
                TxtReleaseDate.Text = commissionToEdit.ReleaseDate.ToString("yyyy-MM-dd");

                EditCommissionOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseEditOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditCommissionOverlay.Visibility = Visibility.Collapsed;
            commissionToEdit = null;
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            LogsPage logsPage = new LogsPage();
            this.NavigationService?.Navigate(logsPage);
        }

        // --- SEARCH BAR ---
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.Trim().ToLower();

            var filtered = allCommissionsModels
                .Where(c =>
                    ("C" + c.CommissionId.ToString("D3")).ToLower().Contains(query) ||
                    ("S" + c.SalesId.ToString("D3")).ToLower().Contains(query) ||
                    ("A" + c.AgentId.ToString("D3")).ToLower().Contains(query) ||
                    c.CommissionAmount.ToString("F2").Contains(query) ||
                    c.ReleaseDate.ToString("yyyy-MM-dd").Contains(query))
                .ToList();

            RefreshDataGrid(filtered);

            PlaceholderText.Visibility = string.IsNullOrEmpty(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    // --- RAW MODEL ---
    public class Commission
    {
        public int CommissionId { get; set; }
        public int SalesId { get; set; }
        public int AgentId { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    // --- DISPLAY MODEL ---
    public class CommissionDisplayItem
    {
        public string CommissionID { get; set; }
        public string SalesID { get; set; }
        public string AgentID { get; set; }
        public string CommissionAmount { get; set; }
        public string ReleaseDate { get; set; }

    }
}
