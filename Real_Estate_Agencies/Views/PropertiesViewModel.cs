using System.Collections.ObjectModel;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies.Views
{
    public class PropertiesViewModel
    {
        public ObservableCollection<PropertyModel> Properties { get; set; }

        public PropertiesViewModel()
        {
            Properties = new ObservableCollection<PropertyModel>
            {
                new PropertyModel
                {
                    Name = "Ocean View Condo",
                    Location = "Miami",
                    PropertyType = "Condo",
                    Category = "Residential",
                    Price = 350000,
                    Status = "On Sale",
                    ImagePath = "Images/default.jpg"
                },
                new PropertyModel
                {
                    Name = "Downtown Apartment",
                    Location = "New York",
                    PropertyType = "Apartment",
                    Category = "Residential",
                    Price = 500000,
                    Status = "Sold",
                    ImagePath = "Images/default.jpg"
                },
                new PropertyModel
                {
                    Name = "Beach House",
                    Location = "California",
                    PropertyType = "House",
                    Category = "Vacation",
                    Price = 750000,
                    Status = "Pending",
                    ImagePath = "Images/default.jpg"
                }
            };
        }
    }
}
