using Real_Estate_Agencies.Model;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Real_Estate_Agencies
{
    public partial class AddClientWindow : Window
    {
        public Client NewClient { get; set; }

        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void AddClientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade + Scale animation
            var fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            MainCard.BeginAnimation(OpacityProperty, fadeAnim);

            var scaleAnim = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            MainCard.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleAnim);
            MainCard.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                NewClient = new Client
                {
                    ClientId = int.TryParse(TxtClientId.Text, out int id) ? id : 0,
                    FirstName = TxtFirstName.Text.Trim(),
                    LastName = TxtLastName.Text.Trim(),
                    ContactInfo = TxtContactInfo.Text.Trim(),
                    Address = TxtAddress.Text.Trim(),
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                MessageBox.Show("Please enter First Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("Please enter Last Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtContactInfo.Text))
            {
                MessageBox.Show("Please enter Contact Info.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                MessageBox.Show("Please enter Address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
