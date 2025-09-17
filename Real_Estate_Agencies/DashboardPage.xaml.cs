using System.Windows.Controls;
using System.Windows.Input; // for MouseWheelEventArgs
using Real_Estate_Agencies.Models; // for DashboardViewModel

namespace Real_Estate_Agencies
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();

            // Bind the page to the ViewModel
            DataContext = new DashboardViewModel();
        }

        // Handle horizontal scrolling of the property list using mouse wheel
        private void PropertyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                // Scroll horizontally instead of vertically
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }
    }
}
