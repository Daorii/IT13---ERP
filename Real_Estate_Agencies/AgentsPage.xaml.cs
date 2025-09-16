using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AgentsPage : Page
    {
        public AgentsPage()
        {
            InitializeComponent();
            LoadAgents();
        }

        private void LoadAgents()
        {
            // Example dummy data
            AgentsDataGrid.ItemsSource = new List<Agent>
            {
                new Agent { EmployeeId = "A001", FirstName = "Maria", LastName = "Santos", HireDate = "2021-06-15", ContactInfo = "09171234567", SalaryRate = "₱30,000" },
                new Agent { EmployeeId = "A002", FirstName = "John", LastName = "Reyes", HireDate = "2022-03-10", ContactInfo = "09179876543", SalaryRate = "₱28,000" }
            };
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow addAgentWindow = new AddAgentWindow();
            addAgentWindow.ShowDialog();

            // After closing the modal, reload data (later can be hooked to DB)
            LoadAgents();
        }
    }

    public class Agent
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HireDate { get; set; }
        public string ContactInfo { get; set; }
        public string SalaryRate { get; set; }
    }
}
