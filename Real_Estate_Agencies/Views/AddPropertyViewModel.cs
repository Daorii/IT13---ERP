using System.ComponentModel;
using System.Windows.Input;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Common; // Add this using directive

namespace Real_Estate_Agencies.ViewModels
{
    public class AddPropertyViewModel : INotifyPropertyChanged
    {
        private PropertyModel _newProperty;
        public PropertyModel NewProperty
        {
            get => _newProperty;
            set
            {
                _newProperty = value;
                OnPropertyChanged(nameof(NewProperty));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddPropertyViewModel()
        {
            NewProperty = new PropertyModel();

            // CORRECT: Initialize the read-only commands here in the constructor
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            // You will handle closing the window from the AddPropertyWindow.xaml.cs code-behind
            // when the button is clicked and this command is executed.
        }

        private void Cancel()
        {
            // Same as above.
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}