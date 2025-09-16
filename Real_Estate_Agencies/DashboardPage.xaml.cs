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

            // Bind the page to the ViewModel (make sure it exists under Models folder)
            DataContext = new DashboardViewModel();
        }

        // Handle mouse wheel scrolling for property list
        private void PropertyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }
    }
}
