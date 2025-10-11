using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class IncentivesPage : Page
    {
        private List<IncentiveDisplayItem> allIncentives;
        private List<IncentiveDisplayItem> filteredIncentives;
        private readonly IncentivesRepository repo;

        public IncentivesPage()
        {
            InitializeComponent();
            repo = new IncentivesRepository();
            LoadIncentives();

            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            TogglePlaceholder();
        }

        private void LoadIncentives()
        {
            var incentives = repo.GetAllIncentives();
            allIncentives = incentives.Select(ConvertToDisplayItem).ToList();
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
            if (addWindow.ShowDialog() == true && addWindow.NewIncentive != null)
            {
                repo.AddIncentive(addWindow.NewIncentive);
                allIncentives.Add(ConvertToDisplayItem(addWindow.NewIncentive));
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
                TxtAgentName.Text = incentive.AgentName;
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
            try
            {
                int incentiveId = int.Parse(TxtIncentiveId.Text.Replace("I", ""));
                var existing = allIncentives.FirstOrDefault(x => x.IncentiveId == "I" + incentiveId.ToString("D3"));
                if (existing == null)
                {
                    MessageBox.Show("Incentive not found.");
                    return;
                }

                string incentiveType = CmbIncentiveType.Text;
                decimal amount = decimal.Parse(TxtAmount.Text);
                DateTime releaseDate = DateTime.Parse(TxtReleaseDate.Text);
                int agentId = int.Parse(existing.AgentId);

                var updatedIncentive = new Incentive
                {
                    IncentiveId = incentiveId,
                    AgentId = agentId,
                    IncentiveType = incentiveType,
                    Amount = amount,
                    ReleaseDate = releaseDate
                };

                repo.UpdateIncentive(updatedIncentive);

                existing.IncentiveType = incentiveType;
                existing.Amount = "₱" + amount.ToString("N0");
                existing.ReleaseDate = releaseDate.ToString("yyyy-MM-dd");

                ApplyFilter(SearchTextBox.Text);
                MessageBox.Show("Incentive updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating incentive: {ex.Message}");
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
                    int incentiveId = int.Parse(incentive.IncentiveId.Replace("I", ""));
                    repo.DeleteIncentive(incentiveId);
                    allIncentives.Remove(incentive);
                    ApplyFilter(SearchTextBox.Text);

                    MessageBox.Show("Incentive deleted successfully!");
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TogglePlaceholder();
            ApplyFilter(SearchTextBox.Text);
        }

        private void TogglePlaceholder()
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
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
                             || (x.AgentName != null && x.AgentName.ToLower().Contains(searchText))
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
                AgentId = incentive.AgentId.ToString(),
                AgentName = incentive.AgentName,
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
        public string AgentName { get; set; }
        public string IncentiveType { get; set; }
        public string Amount { get; set; }
        public string ReleaseDate { get; set; }
    }
}