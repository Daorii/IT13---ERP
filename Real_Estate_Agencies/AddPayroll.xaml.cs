using System;
using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class AddPayroll : Window
    {
        public Payroll NewPayroll { get; set; }

        public AddPayroll()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int agentId = int.Parse(TxtAgentId.Text);
                string payPeriod = TxtPayPeriod.Text;
                decimal.TryParse(TxtCommissionSalary.Text, out decimal commissionSalary);
                decimal.TryParse(TxtNetPay.Text, out decimal netPay);

                NewPayroll = new Payroll
                {
                    AgentId = agentId,
                    PayPeriod = payPeriod,
                    CommissionSalary = commissionSalary,
                    NetPay = netPay
                };

                MessageBox.Show("Payroll saved successfully!", "Success");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
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
    }
}
