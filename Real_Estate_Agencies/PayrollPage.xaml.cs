using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PayrollPage : Page
    {
        private ObservableCollection<Payroll> payrolls;

        public PayrollPage()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            payrolls = new ObservableCollection<Payroll>
            {
                new Payroll { PayrollId = 1, AgentId = 1, PayPeriod = "Jan 2024", CommissionSalary = 50000, NetPay = 45000 },
                new Payroll { PayrollId = 2, AgentId = 2, PayPeriod = "Jan 2024", CommissionSalary = 55000, NetPay = 48000 },
                new Payroll { PayrollId = 3, AgentId = 3, PayPeriod = "Feb 2024", CommissionSalary = 60000, NetPay = 54000 }
            };

            PayrollDataGrid.ItemsSource = payrolls;
        }

        private void AddPayroll_Click(object sender, RoutedEventArgs e)
        {
            var addPayrollWindow = new AddPayroll();
            if (addPayrollWindow.ShowDialog() == true && addPayrollWindow.NewPayroll != null)
            {
                payrolls.Add(addPayrollWindow.NewPayroll);
            }
        }
    }
}
