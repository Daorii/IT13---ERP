using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class CommissionsPage : Page
    {
        private List<Commission> allCommissionsModels;
        private Commission commissionToEdit;
        private static int nextCommissionId = 4;

        public CommissionsPage()
        {
            InitializeComponent();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            allCommissionsModels = new List<Commission>
            {
                new Commission { CommissionId = 1, SalesId = 1, AgentId = 1, CommissionAmount = 125000, ReleaseDate = new DateTime(2024, 1, 15) },
                new Commission { CommissionId = 2, SalesId = 2, AgentId = 2, CommissionAmount = 139500, ReleaseDate = new DateTime(2024, 2, 20) },
                new Commission { CommissionId = 3, SalesId = 3, AgentId = 3, CommissionAmount = 200000, ReleaseDate = new DateTime(2024, 3, 10) }
            };

            RefreshDataGrid();
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

    public class CommissionDisplayItem
    {
        public string CommissionID { get; set; }
        public string SalesID { get; set; }
        public string AgentID { get; set; }
        public string CommissionAmount { get; set; }
        public string ReleaseDate { get; set; }
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
