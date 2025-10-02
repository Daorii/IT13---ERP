using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class IncentivesPage : Page
    {
        private List<IncentiveDisplayItem> allIncentives;
        private List<IncentiveDisplayItem> filteredIncentives;
        private static int nextIncentiveId = 3;

        public IncentivesPage()
        {
            InitializeComponent();
            LoadIncentives();
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        }

        private void LoadIncentives()
        {
            allIncentives = new List<IncentiveDisplayItem>
            {
                new IncentiveDisplayItem { IncentiveId = "I001", AgentId = "A001", IncentiveType = "Bonus", Amount = "₱5,000", ReleaseDate = "2024-01-20" },
                new IncentiveDisplayItem { IncentiveId = "I002", AgentId = "A002", IncentiveType = "Travel Allowance", Amount = "₱3,000", ReleaseDate = "2024-02-15" }
            };

            filteredIncentives = new List<IncentiveDisplayItem>(allIncentives);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            IncentivesDataGrid.ItemsSource = null;
            IncentivesDataGrid.ItemsSource = filteredIncentives;
        }

        private void AddIncentive_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddIncentive();
            bool? result = addWindow.ShowDialog();

            if (result == true && addWindow.NewIncentive != null)
            {
                addWindow.NewIncentive.IncentiveId = nextIncentiveId++;
                var displayItem = ConvertToDisplayItem(addWindow.NewIncentive);
                allIncentives.Add(displayItem);
                ApplyFilter(SearchTextBox.Text);
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
                TxtAmount.Text = incentive.Amount.Replace("₱", "").Replace(",", "");
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
            var incentive = allIncentives.FirstOrDefault(x => x.IncentiveId == TxtIncentiveId.Text);
            if (incentive != null)
            {
                incentive.AgentId = TxtAgentId.Text;
                incentive.IncentiveType = CmbIncentiveType.Text;
                incentive.Amount = "₱" + decimal.Parse(TxtAmount.Text).ToString("N0");
                incentive.ReleaseDate = TxtReleaseDate.Text;

                ApplyFilter(SearchTextBox.Text);
                MessageBox.Show("Incentive updated successfully!");
            }

            EditIncentiveOverlay.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var incentive = button?.CommandParameter as IncentiveDisplayItem;

            if (incentive != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {incentive.IncentiveId}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    allIncentives.Remove(incentive);
                    ApplyFilter(SearchTextBox.Text);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter(SearchTextBox.Text);
        }

        private void ApplyFilter(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredIncentives = new List<IncentiveDisplayItem>(allIncentives);
            }
            else
            {
                searchText = searchText.ToLower();
                filteredIncentives = allIncentives
                    .Where(x => x.IncentiveId.ToLower().Contains(searchText)
                             || (x.AgentId != null && x.AgentId.ToLower().Contains(searchText))
                             || (x.IncentiveType != null && x.IncentiveType.ToLower().Contains(searchText)))
                    .ToList();
            }
            RefreshGrid();
        }

        private IncentiveDisplayItem ConvertToDisplayItem(Incentive incentive)
        {
            return new IncentiveDisplayItem
            {
                IncentiveId = "I" + incentive.IncentiveId.ToString("D3"),
                AgentId = "A" + incentive.AgentId.ToString("D3"),
                IncentiveType = incentive.IncentiveType,
                Amount = "₱" + incentive.Amount.ToString("N0"),
                ReleaseDate = incentive.ReleaseDate.ToString("yyyy-MM-dd")
            };
        }
    }

    public class IncentiveDisplayItem
    {
        public string IncentiveId { get; set; }
        public string AgentId { get; set; }
        public string IncentiveType { get; set; }
        public string Amount { get; set; }
        public string ReleaseDate { get; set; }
    }
}
