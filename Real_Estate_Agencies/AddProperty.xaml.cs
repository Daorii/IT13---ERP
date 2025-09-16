using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class AddPropertyWindow : Window
    {
        public PropertyModel NewProperty { get; private set; }

        public AddPropertyWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            NewProperty = new PropertyModel
            {
                Name = NameInput.Text,
                Location = LocationInput.Text,
                Type = TypeInput.Text,
                Category = CategoryInput.Text,
                Price = PriceInput.Text,
                Status = StatusInput.Text
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
