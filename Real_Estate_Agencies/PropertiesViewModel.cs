using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Common;

namespace Real_Estate_Agencies.ViewModels
{
    public class PropertiesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PropertyModel> _properties;
        private PropertyModel _selectedProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PropertyModel> Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }

        public PropertyModel SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                _selectedProperty = value;
                OnPropertyChanged(nameof(SelectedProperty));
                ((RelayCommand)EditPropertyCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeletePropertyCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPropertyCommand { get; }
        public ICommand EditPropertyCommand { get; }
        public ICommand DeletePropertyCommand { get; }

        public PropertiesViewModel()
        {
            Properties = new ObservableCollection<PropertyModel>
            {
                new PropertyModel
                {
                    Id = 1,
                    Code = "P001",
                    Name = "Tavares Cliffs",
                    Location = "089 Grant Overpass",
                    PropertyType = "House",
                    Category = "Residential",
                    Price = 500000,
                    Status = "On Sale",
                    ImagePath = "Images/default.jpg",
                    No = 1
                },
                new PropertyModel
                {
                    Id = 2,
                    Code = "P002",
                    Name = "Ocean View Condo",
                    Location = "123 Miami Beach",
                    PropertyType = "Condo",
                    Category = "Residential",
                    Price = 350000,
                    Status = "Pending",
                    ImagePath = "Images/default.jpg",
                    No = 2
                }
            };

            AddPropertyCommand = new RelayCommand(AddProperty);
            EditPropertyCommand = new RelayCommand(EditProperty, CanEditOrDelete);
            DeletePropertyCommand = new RelayCommand(DeleteProperty, CanEditOrDelete);
        }

        private void AddProperty()
        {
            var addWindow = new Real_Estate_Agencies.Views.AddPropertyWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                // Assign a new No for display
                int maxNo = Properties.Count > 0 ? Properties[^1].No : 0;
                addWindow.NewProperty.No = maxNo + 1;

                Properties.Add(addWindow.NewProperty);
                MessageBox.Show("New property added successfully!");
            }
        }

        private void EditProperty()
        {
            if (SelectedProperty != null)
            {
                MessageBox.Show($"Editing property: {SelectedProperty.Name}");
                // Optional: open an EditPropertyWindow
            }
        }

        private void DeleteProperty()
        {
            if (SelectedProperty != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{SelectedProperty.Name}'?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    Properties.Remove(SelectedProperty);
                    MessageBox.Show("Property deleted successfully.");
                }
            }
        }

        private bool CanEditOrDelete() => SelectedProperty != null;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
