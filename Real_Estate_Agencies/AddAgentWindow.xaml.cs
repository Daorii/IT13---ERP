using System;
using System.Windows;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class AddAgentWindow : Window
    {
        public Agent NewAgent { get; private set; }

        public AddAgentWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                string.IsNullOrWhiteSpace(TxtContactInfo.Text) ||
                DpHireDate.SelectedDate == null)
            {
                MessageBox.Show("All fields are required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Build new Agent object (AgentID is auto-increment in DB)
            NewAgent = new Agent
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                ContactInfo = TxtContactInfo.Text.Trim(),
                HireDate = DpHireDate.SelectedDate?.ToString("yyyy-MM-dd") ?? ""
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
