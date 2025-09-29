using Real_Estate_Agencies.Common;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
    
namespace Real_Estate_Agencies.ViewModels
{
    public class PropertiesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PropertyModel> _properties;
        private PropertyModel _selectedProperty;
        private readonly PropertyRepository _repository;

        // 🔹 Add these missing private fields
        private ObservableCollection<string> _categories;
        private ObservableCollection<string> _propertyTypes;
        private string _selectedCategory;


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

        // 🔹 Expose Categories and PropertyTypes to XAML
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public ObservableCollection<string> PropertyTypes
        {
            get => _propertyTypes;
            set
            {
                _propertyTypes = value;
                OnPropertyChanged(nameof(PropertyTypes));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                UpdatePropertyTypes(); // refresh property types when category changes
            }
        }


        public ICommand AddPropertyCommand { get; }
        public ICommand EditPropertyCommand { get; }
        public ICommand DeletePropertyCommand { get; }

        public PropertiesViewModel()
        {
            _repository = new PropertyRepository();

            // Load properties from the database
            var propertiesFromDb = _repository.GetAll();
            Properties = new ObservableCollection<PropertyModel>(propertiesFromDb);

            AddPropertyCommand = new RelayCommand(AddProperty);
            EditPropertyCommand = new RelayCommand(EditProperty, CanEditOrDelete);
            DeletePropertyCommand = new RelayCommand(DeleteProperty, CanEditOrDelete);

            // 🔹 Initialize Categories
            Categories = new ObservableCollection<string>
            {
                "Residential",
                "Commercial",
                "Industrial",
                "Lot Only",
                "Agricultural",
                "Mixed-Use"
            };

            PropertyTypes = new ObservableCollection<string>(); // starts empty
        }

        

        private void AddProperty()
        {
            var addWindow = new Real_Estate_Agencies.Views.AddPropertyWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                int maxNo = Properties.Count > 0 ? Properties[^1].No : 0;
                addWindow.NewProperty.No = maxNo + 1;

                // Save to DB
                _repository.Add(addWindow.NewProperty);

                // Add to observable collection
                Properties.Add(addWindow.NewProperty);
                MessageBox.Show("New property added successfully!");
            }
        }

        private void EditProperty()
        {
            if (SelectedProperty != null)
            {
                MessageBox.Show($"Editing {SelectedProperty.Name}");
                // TODO: open Edit overlay/window
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
                    // Delete from DB
                    _repository.Delete(SelectedProperty.Id);

                    // Remove from collection
                    Properties.Remove(SelectedProperty);
                    MessageBox.Show("Property deleted successfully.");
                }
            }
        }

        private bool CanEditOrDelete() => SelectedProperty != null;


        // 🔹 Update PropertyTypes when Category changes
        private void UpdatePropertyTypes()
        {
            PropertyTypes.Clear();

            switch (SelectedCategory)
            {
                case "Residential":
                    PropertyTypes.Add("Condominium – Studio");
                    PropertyTypes.Add("Condominium – 1BR");
                    PropertyTypes.Add("Condominium – 2BR");
                    PropertyTypes.Add("Condominium – 3BR");
                    PropertyTypes.Add("Condominium – Penthouse");
                    PropertyTypes.Add("Apartment Unit");
                    PropertyTypes.Add("Townhouse");
                    PropertyTypes.Add("Single-Detached House");
                    PropertyTypes.Add("Duplex");
                    PropertyTypes.Add("Rowhouse");
                    break;

                case "Commercial":
                    PropertyTypes.Add("Commercial Space (Retail)");
                    PropertyTypes.Add("Office Space");
                    PropertyTypes.Add("Restaurant Space");
                    PropertyTypes.Add("Showroom");
                    PropertyTypes.Add("Co-Working Space");
                    break;

                case "Industrial":
                    PropertyTypes.Add("Warehouse");
                    PropertyTypes.Add("Factory");
                    PropertyTypes.Add("Manufacturing Plant");
                    PropertyTypes.Add("Distribution Center");
                    break;

                case "Lot Only":
                    PropertyTypes.Add("Vacant Lot – Residential");
                    PropertyTypes.Add("Vacant Lot – Commercial");
                    PropertyTypes.Add("Vacant Lot – Mixed-Use");
                    break;

                case "Agricultural":
                    PropertyTypes.Add("Farm Lot");
                    PropertyTypes.Add("Plantation Land");
                    PropertyTypes.Add("Agro-Industrial Lot");
                    break;

                case "Mixed-Use":
                    PropertyTypes.Add("Residential + Commercial Building");
                    PropertyTypes.Add("Commercial/Residential Lot");
                    PropertyTypes.Add("Shop-Top Housing (store on ground floor, unit upstairs)");
                    break;
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
