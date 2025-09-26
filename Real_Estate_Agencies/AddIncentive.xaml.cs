using Real_Estate_Agencies.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AddIncentive : Window
    {
        public Incentive NewIncentive { get; set; }  // Add this property

        public AddIncentive()
        {
            InitializeComponent();
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                NewIncentive = new Incentive
                {
                    AgentId = int.Parse(TxtAgentId.Text),
                    IncentiveType = TxtIncentiveType.Text,
                    Amount = decimal.Parse(TxtAmount.Text),
                    Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Pending",
                    ReleaseDate = DateTime.Parse(TxtReleaseDate.Text)
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
                MessageBox.Show("Please enter a valid Agent ID.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtIncentiveType.Text))
            {
                MessageBox.Show("Please enter Incentive Type.");
                return false;
            }
            if (!decimal.TryParse(TxtAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid amount.");
                return false;
            }
            return true;
        }
    }
}