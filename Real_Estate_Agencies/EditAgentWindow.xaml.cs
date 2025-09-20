using Real_Estate_Agencies.Model;
using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class EditAgentWindow : Window
    {
        public EditAgentWindow()
        {
            InitializeComponent();
        }

        // Load agent data into the form
        public void LoadAgent(AgentModel agent)
        {
            if (agent == null) return;

            TxtEmployeeId.Text = agent.EmployeeId.ToString();
            TxtFirstName.Text = agent.FirstName;
            TxtLastName.Text = agent.LastName;
            TxtContactInfo.Text = agent.ContactInfo;
            DpHireDate.SelectedDate = agent.HireDate;
            TxtSalaryRate.Text = agent.SalaryRate.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Example validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("First and Last Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update logic here
            AgentModel updatedAgent = new AgentModel
            {
                EmployeeId = int.Parse(TxtEmployeeId.Text),
                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                ContactInfo = TxtContactInfo.Text,
                HireDate = DpHireDate.SelectedDate ?? System.DateTime.Now,
                SalaryRate = decimal.TryParse(TxtSalaryRate.Text, out decimal rate) ? rate : 0
            };

            // Call your database update method
            // Database.UpdateAgent(updatedAgent);

            MessageBox.Show("Agent updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
