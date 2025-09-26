using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;

namespace Real_Estate_Agencies
{
    public partial class DashboardPage : Page
    {
        private DispatcherTimer _scrollTimer;
        private double _scrollSpeed = 1; // pixels per tick
        private bool _scrollingRight = true; // direction

        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }

        // Scroll with mouse wheel horizontally
        private void PropertyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }

        // Start auto-scrolling on load
        private void TopPropertiesScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20) // 50 FPS
            };
            _scrollTimer.Tick += ScrollTimer_Tick;
            _scrollTimer.Start();

            // Pause on hover
            TopPropertiesScrollViewer.MouseEnter += (s, ev) => _scrollTimer.Stop();
            TopPropertiesScrollViewer.MouseLeave += (s, ev) => _scrollTimer.Start();
        }

        // Scroll logic
        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (TopPropertiesScrollViewer.ScrollableWidth == 0) return;

            double newOffset = TopPropertiesScrollViewer.HorizontalOffset;

            if (_scrollingRight)
            {
                newOffset += _scrollSpeed;
                if (newOffset >= TopPropertiesScrollViewer.ScrollableWidth)
                    _scrollingRight = false;
            }
            else
            {
                newOffset -= _scrollSpeed;
                if (newOffset <= 0)
                    _scrollingRight = true;
            }

            TopPropertiesScrollViewer.ScrollToHorizontalOffset(newOffset);
        }
    }
}