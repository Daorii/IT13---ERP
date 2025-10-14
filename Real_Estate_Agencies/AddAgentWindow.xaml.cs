using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class AddAgentWindow : Window
    {
        public Agent NewAgent { get; private set; }
        public string ImagePath { get; private set; } // Store uploaded image path
        private bool isEditMode = false;
        public string AddButtonText => isEditMode ? "Save" : "Add";


        public AddAgentWindow()
        {
            InitializeComponent();
            NewAgent = new Agent();
        }

        // New constructor for Edit mode
        public AddAgentWindow(Agent agentToEdit) : this()
        {
            if (agentToEdit != null)
            {
                isEditMode = true;
                NewAgent = new Agent
                {
                    AgentId = agentToEdit.AgentId,
                    FirstName = agentToEdit.FirstName,
                    LastName = agentToEdit.LastName,
                    ContactInfo = agentToEdit.ContactInfo,
                    HireDate = agentToEdit.HireDate,
                    ProfileImage = agentToEdit.ProfileImage // use bytes instead of ImagePath
                };

                // Fill form fields with existing data
                TxtFirstName.Text = NewAgent.FirstName;
                TxtLastName.Text = NewAgent.LastName;
                TxtContactInfo.Text = NewAgent.ContactInfo;
                DpHireDate.SelectedDate = DateTime.TryParse(NewAgent.HireDate, out var dt) ? dt : (DateTime?)null;

                // Load profile image from database bytes
                if (NewAgent.ProfileImage != null)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(NewAgent.ProfileImage);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    PreviewImage.Source = bitmap;
                    PreviewImage.Visibility = Visibility.Visible;
                }
            }
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Agent Profile Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                NewAgent.ProfileImage = File.ReadAllBytes(path); // store bytes

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(NewAgent.ProfileImage);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                PreviewImage.Source = bitmap;
                PreviewImage.Visibility = Visibility.Visible;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                string.IsNullOrWhiteSpace(TxtContactInfo.Text) ||
                DpHireDate.SelectedDate == null)
            {
                MessageBox.Show("All fields are required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate contact info - must be numbers only
            string contactInfo = TxtContactInfo.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(contactInfo, @"^[0-9\-\+\(\)\s]+$"))
            {
                MessageBox.Show("Contact Info must contain only numbers and phone formatting characters (+, -, (, ), space).",
                               "Invalid Contact Info",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                TxtContactInfo.Focus();
                return;
            }

            // Additional check: ensure it has at least some digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(contactInfo, @"\d"))
            {
                MessageBox.Show("Contact Info must contain at least one number.",
                               "Invalid Contact Info",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                TxtContactInfo.Focus();
                return;
            }

            // Count only digits (exclude formatting characters)
            string digitsOnly = System.Text.RegularExpressions.Regex.Replace(contactInfo, @"[^\d]", "");

            // Philippine phone number validation
            if (digitsOnly.Length < 7)
            {
                MessageBox.Show("Contact Info must have at least 7 digits (Philippine landline minimum).",
                               "Invalid Contact Info",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                TxtContactInfo.Focus();
                return;
            }

            if (digitsOnly.Length > 13)
            {
                MessageBox.Show("Contact Info cannot exceed 13 digits (Philippine mobile with country code: +63 9xx xxx xxxx).",
                               "Invalid Contact Info",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                TxtContactInfo.Focus();
                return;
            }

            // Confirmation dialog before saving
            string confirmMessage = isEditMode
                ? "Are you sure you want to save changes to this agent?"
                : "Are you sure you want to add this new agent?";

            var result = MessageBox.Show(confirmMessage,
                                        "Confirm Action",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return; // Cancel the operation
            }

            // Set properties
            NewAgent.FirstName = TxtFirstName.Text.Trim();
            NewAgent.LastName = TxtLastName.Text.Trim();
            NewAgent.ContactInfo = TxtContactInfo.Text.Trim();
            NewAgent.HireDate = DpHireDate.SelectedDate?.ToString("yyyy-MM-dd") ?? "";

            DialogResult = true;
            Close();
        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Check if user has entered any data
            bool hasData = !string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                          !string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                          !string.IsNullOrWhiteSpace(TxtContactInfo.Text) ||
                          DpHireDate.SelectedDate != null ||
                          PreviewImage.Source != null;

            if (hasData)
            {
                var result = MessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.",
                                           "Confirm Cancel",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return; // Don't close the window
                }
            }

            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // Check if user has entered any data
            bool hasData = !string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                          !string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                          !string.IsNullOrWhiteSpace(TxtContactInfo.Text) ||
                          DpHireDate.SelectedDate != null ||
                          PreviewImage.Source != null;

            if (hasData)
            {
                var result = MessageBox.Show("Are you sure you want to close? All unsaved changes will be lost.",
                                           "Confirm Close",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return; // Don't close the window
                }
            }

            DialogResult = false;
            Close();
        }
    }
}