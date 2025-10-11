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

            // DO NOT overwrite NewAgent in Add mode
            // if (!isEditMode)
            //     NewAgent = new Agent(); // REMOVE this

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
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
