using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies.Views
{
    public partial class StatusBadge : UserControl
    {
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(StatusBadge),
                new PropertyMetadata(string.Empty));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public StatusBadge()
        {
            InitializeComponent();
        }
    }
}