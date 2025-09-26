using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies.Models;

namespace Real_Estate_Agencies
{
    public partial class CommissionsPage : Page
    {
        private List<Commission> allCommissionsModels;
        private Commission commissionToEdit;
        private static int nextCommissionId = 4;
        private static int nextPaymentId = 4;

        public CommissionsPage()
        {
            InitializeComponent();
            LoadCommissions();
        }

        private void LoadCommissions()
        {
            // Use the actual data model as the source of truth
            allCommissionsModels = new List<Commission>
            {
                new Commission { CommissionId = 1, SalesId = 1, AgentId = 1, PaymentId = 1, CommissionRate = 5.0m, CommissionAmount = 125000, Status = "Paid", ReleaseDate = new DateTime(2024, 1, 15) },
                new Commission { CommissionId = 2, SalesId = 2, AgentId = 2, PaymentId = 2, CommissionRate = 4.5m, CommissionAmount = 139500, Status = "Pending", ReleaseDate = new DateTime(2024, 2, 20) },
                new Commission { CommissionId = 3, SalesId = 3, AgentId = 3, PaymentId = 3, CommissionRate = 5.0m, CommissionAmount = 200000, Status = "Processing", ReleaseDate = new DateTime(2024, 3, 10) }
            };

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            // Convert the data models to display models whenever the grid needs to be updated
            CommissionsDataGrid.ItemsSource = null;
            CommissionsDataGrid.ItemsSource = allCommissionsModels.Select(c => ConvertToDisplayItem(c)).ToList();
        }

        private void AddCommission_Click(object sender, RoutedEventArgs e)
        {
            // This logic can be adapted to use an overlay as well if desired
            try
            {
                var addWindow = new AddCommission();
                bool? result = addWindow.ShowDialog();

                if (result == true && addWindow.NewCommission != null)
                {
                    addWindow.NewCommission.CommissionId = nextCommissionId++;
                    addWindow.NewCommission.PaymentId = nextPaymentId++;
                    allCommissionsModels.Add(addWindow.NewCommission);
                    RefreshDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var displayItem = button.CommandParameter as CommissionDisplayItem;
            if (displayItem == null) return;

            // Find the original model object to edit using the ID from the display item
            int commissionId = int.Parse(displayItem.CommissionID.Substring(1));
            commissionToEdit = allCommissionsModels.FirstOrDefault(c => c.CommissionId == commissionId);

            if (commissionToEdit != null)
            {
                // Populate the overlay fields from the data model
                TxtCommissionId.Text = "C" + commissionToEdit.CommissionId.ToString("D3");
                TxtSalesId.Text = "S" + commissionToEdit.SalesId.ToString("D3");
                TxtAgentId.Text = "A" + commissionToEdit.AgentId.ToString("D3");
                TxtCommissionAmount.Text = commissionToEdit.CommissionAmount.ToString("C", new CultureInfo("en-PH"));
                TxtReleaseDate.Text = commissionToEdit.ReleaseDate.ToString("yyyy-MM-dd");
                TxtCommissionRate.Text = commissionToEdit.CommissionRate.ToString("F1");

                // Set the selected item in the ComboBox
                foreach (ComboBoxItem item in CmbStatus.Items)
                {
                    if (item.Content.ToString() == commissionToEdit.Status)
                    {
                        CmbStatus.SelectedItem = item;
                        break;
                    }
                }

                // Show the overlay
                EditCommissionOverlay.Visibility = Visibility.Visible;
            }
        }

        private void UpdateCommission_Click(object sender, RoutedEventArgs e)
        {
            if (commissionToEdit == null) return;

            // Validate input
            if (!decimal.TryParse(TxtCommissionRate.Text, out decimal rate) || rate < 0)
            {
                MessageBox.Show("Please enter a valid commission rate.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the original data model
            commissionToEdit.CommissionRate = rate;
            commissionToEdit.Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Refresh the entire grid to reflect changes
            RefreshDataGrid();

            // Close the overlay
            CloseEditOverlay_Click(null, null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var displayItem = button.CommandParameter as CommissionDisplayItem;
            if (displayItem == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete commission {displayItem.CommissionID}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                int commissionId = int.Parse(displayItem.CommissionID.Substring(1));
                var commissionToDelete = allCommissionsModels.FirstOrDefault(c => c.CommissionId == commissionId);
                if (commissionToDelete != null)
                {
                    allCommissionsModels.Remove(commissionToDelete);
                    RefreshDataGrid();
                }
            }
        }

        private void CloseEditOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditCommissionOverlay.Visibility = Visibility.Collapsed;
            commissionToEdit = null; // Clear the item being edited
        }

        private CommissionDisplayItem ConvertToDisplayItem(Commission commission)
        {
            // This helper method converts the raw data model to a user-friendly display format
            return new CommissionDisplayItem
            {
                CommissionID = "C" + commission.CommissionId.ToString("D3"),
                SalesID = "S" + commission.SalesId.ToString("D3"),
                AgentID = "A" + commission.AgentId.ToString("D3"),
                PaymentID = "P" + commission.PaymentId.ToString("D3"),
                CommissionRate = commission.CommissionRate.ToString("F1") + "%",
                CommissionAmount = commission.CommissionAmount.ToString("C", new CultureInfo("en-PH")),
                Status = commission.Status,
                ReleaseDate = commission.ReleaseDate.ToString("yyyy-MM-dd")
            };
        }
    }

    // This class is used specifically for displaying data in the grid.
    public class CommissionDisplayItem
    {
        public string CommissionID { get; set; }
        public string SalesID { get; set; }
        public string AgentID { get; set; }
        public string PaymentID { get; set; }
        public string CommissionRate { get; set; }
        public string CommissionAmount { get; set; }
        public string Status { get; set; }
        public string ReleaseDate { get; set; }
    }
}

