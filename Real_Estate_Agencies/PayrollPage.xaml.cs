using System;
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
            new Payroll { PayrollId = 1, AgentId = 1, Period = "Jan 2024", BasicPay = 50000, Deductions = 5000, NetPay = 45000, ReleaseDate = new DateTime(2024, 1, 31) },
            new Payroll { PayrollId = 2, AgentId = 2, Period = "Jan 2024", BasicPay = 55000, Deductions = 7000, NetPay = 48000, ReleaseDate = new DateTime(2024, 1, 31) }
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