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
            // Auto set today's date
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TxtReleaseDate.IsReadOnly = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal.TryParse(TxtBasicPay.Text, out decimal basicPay);
                decimal.TryParse(TxtDeductions.Text, out decimal deductions);
                decimal.TryParse(TxtNetPay.Text, out decimal netPay);
                DateTime.TryParse(TxtReleaseDate.Text, out DateTime releaseDate);

                NewPayroll = new Payroll
                {
                    AgentId = int.Parse(TxtAgentId.Text),
                    Period = TxtPeriod.Text,
                    BasicPay = basicPay,
                    Deductions = deductions,
                    NetPay = netPay,
                    ReleaseDate = releaseDate
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
