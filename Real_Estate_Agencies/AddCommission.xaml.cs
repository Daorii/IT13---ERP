using System;
using System.Windows;
using Real_Estate_Agencies.Models;

namespace Real_Estate_Agencies
{
    public partial class AddCommission : Window
    {
        public Commission NewCommission { get; set; }

        public AddCommission()
        {
            InitializeComponent();
            // Auto-set release date
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TxtReleaseDate.IsReadOnly = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                decimal.TryParse(TxtCommissionRate.Text, out decimal rate);
                DateTime.TryParse(TxtReleaseDate.Text, out DateTime date);

                // Build new commission object
                NewCommission = new Commission
                {
                    CommissionRate = rate,
                    ReleaseDate = date
                };

                MessageBox.Show("Commission added successfully!", "Success");
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

        private bool ValidateInput()
        {
            if (!decimal.TryParse(TxtCommissionRate.Text, out _))
            {
                MessageBox.Show("Please enter a valid commission rate.", "Validation Error");
                return false;
            }

            return true;
        }
    }
}
