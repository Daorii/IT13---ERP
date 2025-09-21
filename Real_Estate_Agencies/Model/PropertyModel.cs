using System.ComponentModel;

namespace Real_Estate_Agencies.Model
{
    public class PropertyModel : INotifyPropertyChanged
    {
        private int _id;
        private string _code;
        private string _name;
        private string _location;
        private string _propertyType;
        private string _category;
        private double _price;
        private string _status;
        private string _imagePath;
        private int _no;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(nameof(Code)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(nameof(Location)); }
        }

        public string PropertyType
        {
            get => _propertyType;
            set { _propertyType = value; OnPropertyChanged(nameof(PropertyType)); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(nameof(Category)); }
        }

        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(nameof(ImagePath)); }
        }

        public int No
        {
            get => _no;
            set { _no = value; OnPropertyChanged(nameof(No)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
