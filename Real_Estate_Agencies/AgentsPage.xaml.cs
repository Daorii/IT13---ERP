using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System;

namespace Real_Estate_Agencies
{
    public partial class AgentsPage : Page
    {
        public ObservableCollection<Agent> Agents { get; set; }
        public Agent SelectedAgent { get; set; }

        public AgentsPage()
        {
            InitializeComponent();
            // Example dummy data
            Agents = new ObservableCollection<Agent>
            {
                new Agent { No = 1, EmployeeId = "A001", FirstName = "Maria", LastName = "Santos", HireDate = "2021-06-15", ContactInfo = "09171234567", SalaryRate = "₱30,000" },
                new Agent { No = 2, EmployeeId = "A002", FirstName = "John", LastName = "Reyes", HireDate = "2022-03-10", ContactInfo = "09179876543", SalaryRate = "₱28,000" }
            };
            DataContext = this;
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow addAgentWindow = new AddAgentWindow();
            addAgentWindow.ShowDialog();
        }

        private void EditAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                SelectedAgent = agent;

                // Populate the form fields with agent data
                EditEmployeeIdTextBox.Text = agent.EmployeeId;
                EditFirstNameTextBox.Text = agent.FirstName;
                EditLastNameTextBox.Text = agent.LastName;
                EditContactInfoTextBox.Text = agent.ContactInfo;
                EditSalaryRateTextBox.Text = agent.SalaryRate;

                // Parse hire date
                if (DateTime.TryParse(agent.HireDate, out DateTime hireDate))
                {
                    EditHireDatePicker.SelectedDate = hireDate;
                }

                EditPopupOverlay.Visibility = Visibility.Visible; // Show edit overlay
            }
            else
            {
                MessageBox.Show("No agent selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(EditEmployeeIdTextBox.Text))
                {
                    MessageBox.Show("Employee ID is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditEmployeeIdTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditFirstNameTextBox.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditFirstNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditLastNameTextBox.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditLastNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditContactInfoTextBox.Text))
                {
                    MessageBox.Show("Contact info is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditContactInfoTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditSalaryRateTextBox.Text))
                {
                    MessageBox.Show("Salary rate is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditSalaryRateTextBox.Focus();
                    return;
                }

                if (EditHireDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Hire date is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditHireDatePicker.Focus();
                    return;
                }

                // If we have a selected agent, update the properties
                if (SelectedAgent != null)
                {
                    SelectedAgent.EmployeeId = EditEmployeeIdTextBox.Text.Trim();
                    SelectedAgent.FirstName = EditFirstNameTextBox.Text.Trim();
                    SelectedAgent.LastName = EditLastNameTextBox.Text.Trim();
                    SelectedAgent.ContactInfo = EditContactInfoTextBox.Text.Trim();
                    SelectedAgent.SalaryRate = EditSalaryRateTextBox.Text.Trim();
                    SelectedAgent.HireDate = EditHireDatePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? "";

                    MessageBox.Show("Agent updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Close the overlay
                    EditPopupOverlay.Visibility = Visibility.Collapsed;

                    // Refresh the data grid to show updated information
                    AgentsDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("No agent selected for editing.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving agent: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopupOverlay.Visibility = Visibility.Collapsed; // Hide edit overlay
        }
    }

    public class Agent
    {
        public int No { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HireDate { get; set; }
        public string ContactInfo { get; set; }
        public string SalaryRate { get; set; }
    }
}