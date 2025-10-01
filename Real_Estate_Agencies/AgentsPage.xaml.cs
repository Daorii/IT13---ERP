using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AgentsPage : Page
    {
        private readonly AgentRepository _repo;

        public ObservableCollection<Agent> Agents { get; set; }
        public Agent SelectedAgent { get; set; }

        public AgentsPage()
        {
            InitializeComponent();

            _repo = new AgentRepository();
            Agents = new ObservableCollection<Agent>(_repo.GetAllAgents());

            DataContext = this;
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow addAgentWindow = new AddAgentWindow();
            if (addAgentWindow.ShowDialog() == true)
            {
                var newAgent = addAgentWindow.NewAgent;
                if (newAgent != null)
                {
                    _repo.AddAgent(newAgent);
                    Agents.Add(newAgent);
                }
            }
        }

        private void EditAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                SelectedAgent = agent;

                EditFirstNameTextBox.Text = agent.FirstName;
                EditLastNameTextBox.Text = agent.LastName;
                EditContactInfoTextBox.Text = agent.ContactInfo;

                if (DateTime.TryParse(agent.HireDate, out DateTime hireDate))
                    EditHireDatePicker.SelectedDate = hireDate;

                EditPopupOverlay.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("No agent selected.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EditFirstNameTextBox.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditLastNameTextBox.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditContactInfoTextBox.Text))
                {
                    MessageBox.Show("Contact info is required.", "Validation Error");
                    return;
                }

                if (EditHireDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Hire date is required.", "Validation Error");
                    return;
                }

                if (SelectedAgent != null)
                {
                    SelectedAgent.FirstName = EditFirstNameTextBox.Text.Trim();
                    SelectedAgent.LastName = EditLastNameTextBox.Text.Trim();
                    SelectedAgent.ContactInfo = EditContactInfoTextBox.Text.Trim();
                    SelectedAgent.HireDate = EditHireDatePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? "";

                    _repo.UpdateAgent(SelectedAgent);

                    MessageBox.Show("Agent updated successfully!", "Success");

                    EditPopupOverlay.Visibility = Visibility.Collapsed;
                    AgentsDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving agent: {ex.Message}", "Error");
            }
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete agent {agent.FirstName} {agent.LastName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repo.DeleteAgent(agent.AgentId);
                        Agents.Remove(agent);

                        MessageBox.Show("Agent deleted successfully.",
                            "Deleted",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting agent: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopupOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
