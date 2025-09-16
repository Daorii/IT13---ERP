using Real_Estate_Agencies;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Real_Estate_Agencies
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load Dashboard by default
            MainFrame.Content = new DashboardPage();
            BtnDashboard.IsChecked = true;
        }

        // Sidebar ToggleButton Checked Event
        private void Sidebar_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clickedButton) return;

            // Uncheck all other buttons
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn != clickedButton)
                    btn.IsChecked = false;
            }

            // Navigate to the corresponding page
            if (clickedButton == BtnDashboard)
                MainFrame.Content = new DashboardPage();
            else if (clickedButton == BtnClients)
                MainFrame.Content = new ClientsPage();
            else if (clickedButton == BtnProperties)
                MainFrame.Content = new PropertiesPage();
            else if (clickedButton == BtnAgents)
                MainFrame.Content = new AgentsPage();
            else if (clickedButton == BtnSales)
                MainFrame.Content = new SalesPage();
            else if (clickedButton == BtnCommissions)
                MainFrame.Content = new CommissionsPage();
            else if (clickedButton == BtnPayroll)
                MainFrame.Content = new PayrollPage();
            else if (clickedButton == BtnIncentives)
                MainFrame.Content = new IncentivesPage();
            else if (clickedButton == BtnIncentives)
                MainFrame.Content = new SalesPage();
        }
    }
}
