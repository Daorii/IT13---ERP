using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies.Models;
using System.Configuration;



namespace Real_Estate_Agencies
{
    public partial class CommissionsPage : Page
    {
        private List<Commission> allCommissionsModels;
        private Commission commissionToEdit;
        //private static int nextCommissionId = 0;

        public CommissionsPage()
        {
            InitializeComponent();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            try
            {
                var repo = new Repository.CommissionRepository();

                var commissions = repo.GetAllCommissions();

                CommissionsDataGrid.ItemsSource = commissions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error (GetAllCommissions): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RefreshDataGrid()
        {
            CommissionsDataGrid.ItemsSource = null;
            CommissionsDataGrid.ItemsSource = allCommissionsModels.Select(c => ConvertToDisplayItem(c)).ToList();
        }

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
    }

   

    public class Commission
    {
        public int CommissionId { get; set; }
        public int SalesId { get; set; }
        public int AgentId { get; set; }
        public decimal CommissionAmount { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
