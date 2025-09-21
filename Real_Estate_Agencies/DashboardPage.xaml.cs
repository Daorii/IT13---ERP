using System.Windows.Controls;
using System.Windows.Input;
using Real_Estate_Agencies.Models;

namespace Real_Estate_Agencies
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }

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