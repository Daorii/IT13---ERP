using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class IncentivesPage : Page
    {
        private List<IncentiveDisplayItem> allIncentives;
        private static int nextIncentiveId = 1;

        public IncentivesPage()
        {
            InitializeComponent();
            LoadIncentives();
        }

        private void LoadIncentives()
        {
            allIncentives = new List<IncentiveDisplayItem>
            {
                new IncentiveDisplayItem
                {
                    IncentiveId = "I001",
                    AgentId = "A001",
                    IncentiveType = "Bonus",
                    Amount = "₱5,000",
                    Status = "Released",
                    ReleaseDate = "2024-01-20"
                },
                new IncentiveDisplayItem
                {
                    IncentiveId = "I002",
                    AgentId = "A002",
                    IncentiveType = "Travel Allowance",
                    Amount = "₱3,000",
                    Status = "Pending",
                    ReleaseDate = "2024-02-15"
                }
            };

            IncentivesDataGrid.ItemsSource = allIncentives;
        }

        private void AddIncentive_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddIncentive();
            bool? result = addWindow.ShowDialog();

            if (result == true && addWindow.NewIncentive != null)
            {
                addWindow.NewIncentive.IncentiveId = nextIncentiveId++;

                // Convert to display format
                var displayItem = ConvertToDisplayItem(addWindow.NewIncentive);
                allIncentives.Add(displayItem);

                IncentivesDataGrid.ItemsSource = null;
                IncentivesDataGrid.ItemsSource = allIncentives;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var incentive = button?.CommandParameter as IncentiveDisplayItem;
            if (incentive != null)
            {
                TxtIncentiveId.Text = incentive.IncentiveId;
                TxtAgentId.Text = incentive.AgentId;
                CmbIncentiveType.Text = incentive.IncentiveType;
                TxtAmount.Text = incentive.Amount;
                CmbStatus.Text = incentive.Status;
                TxtReleaseDate.Text = incentive.ReleaseDate;

                EditIncentiveOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseEditOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditIncentiveOverlay.Visibility = Visibility.Collapsed;
        }

        private void UpdateIncentive_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Update incentive in DB
            MessageBox.Show("Incentive updated successfully!");
            EditIncentiveOverlay.Visibility = Visibility.Collapsed;
        }



        private IncentiveDisplayItem ConvertToDisplayItem(Incentive incentive)
        {
            return new IncentiveDisplayItem
            {
                IncentiveId = "I" + incentive.IncentiveId.ToString().PadLeft(3, '0'),
                AgentId = "A" + incentive.AgentId.ToString().PadLeft(3, '0'),
                IncentiveType = incentive.IncentiveType,
                Amount = "₱" + incentive.Amount.ToString("N0"),
                Status = incentive.Status,
                ReleaseDate = incentive.ReleaseDate.ToString("yyyy-MM-dd")
            };
        }
    }

    // For DataGrid Display
    public class IncentiveDisplayItem
    {
        public string IncentiveId { get; set; }
        public string AgentId { get; set; }
        public string IncentiveType { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string ReleaseDate { get; set; }
    }
}
