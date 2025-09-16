using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class AddAgentWindow : Window
    {
        public AddAgentWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Collect agent details (later save to DB)
            string id = TxtEmployeeId.Text;
            string fname = TxtFirstName.Text;
            string lname = TxtLastName.Text;
            string hireDate = DpHireDate.SelectedDate?.ToString("yyyy-MM-dd") ?? "";
            string contact = TxtContactInfo.Text;
            string salary = TxtSalaryRate.Text;

            MessageBox.Show($"Agent Added:\n{id} - {fname} {lname}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
