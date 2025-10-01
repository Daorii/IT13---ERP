using Real_Estate_Agencies.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AddIncentive : Window
    {
        public Incentive NewIncentive { get; set; }

        public AddIncentive()
        {
            InitializeComponent();
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                NewIncentive = new Incentive
                {
                    AgentId = int.Parse(TxtAgentId.Text),
                    IncentiveType = (CmbIncentiveType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                    Amount = decimal.Parse(TxtAmount.Text),
                    ReleaseDate = DateTime.Parse(TxtReleaseDate.Text)
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidateInput()
        {
            if (!int.TryParse(TxtAgentId.Text, out _))
            {
                MessageBox.Show("Please enter a valid Agent ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CmbIncentiveType.SelectedItem == null)
            {
                MessageBox.Show("Please select an Incentive Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(TxtAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid Amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
