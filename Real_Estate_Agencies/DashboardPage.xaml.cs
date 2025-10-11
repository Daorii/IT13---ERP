using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Effects;
using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class DashboardPage : Page
    {
        private DispatcherTimer _scrollTimer;
        private double _scrollSpeed = 1;
        private bool _scrollingRight = true;
        private DispatcherTimer _clockTimer;

        public DashboardPage()
        {
            InitializeComponent();
            LoadTopSoldProperties();
            DataContext = new DashboardViewModel();

            Loaded += (s, e) =>
            {
                AnimateBars();
                GenerateHourMarks();
                StartAnalogClock();
            };
        }

        // Load top sold properties
        private void LoadTopSoldProperties()
        {
            var repo = new DashboardRepository();
            var topProperties = repo.GetTopSoldProperties(5);
            TopPropertiesItemsControl.ItemsSource = topProperties;
        }

        private void PropertyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }

        private void TopPropertiesScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            _scrollTimer.Tick += ScrollTimer_Tick;
            _scrollTimer.Start();

            TopPropertiesScrollViewer.MouseEnter += (s, ev) => _scrollTimer.Stop();
            TopPropertiesScrollViewer.MouseLeave += (s, ev) => _scrollTimer.Start();
        }

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

        // KPI Card Clicks
        private void PropertySalesCard_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new PropertiesPage());
        }

        private void AgentsCard_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new AgentsPage());
        }

        private void CommissionCard_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new CommissionsPage());
        }

        private void PropertiesCard_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new PropertiesPage());
        }

        // Animate KPI Bars
        private void AnimateBars()
        {
            // Optional: implement bar animations
        }

        private void AnimateBar(Border bar, double progress)
        {
            double maxHeight = 80;
            double targetHeight = progress * maxHeight;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = targetHeight,
                Duration = TimeSpan.FromMilliseconds(600),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            bar.BeginAnimation(HeightProperty, animation);
        }

        // Analog Clock
        private void GenerateHourMarks()
        {
            double centerX = AnalogClock.Width / 2;
            double centerY = AnalogClock.Height / 2;
            double radius = 60;
            int totalMarks = 12;

            for (int i = 0; i < totalMarks; i++)
            {
                double angle = i * 30 * Math.PI / 180;
                double x1 = centerX + radius * Math.Sin(angle);
                double y1 = centerY - radius * Math.Cos(angle);
                double x2 = centerX + (radius - 8) * Math.Sin(angle);
                double y2 = centerY - (radius - 8) * Math.Cos(angle);

                Line mark = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                HourMarks.Children.Add(mark);
            }
        }

        private void StartAnalogClock()
        {
            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (s, e) =>
            {
                DateTime now = DateTime.Now;
                double secAngle = now.Second * 6;
                double minAngle = now.Minute * 6 + secAngle / 60;
                double hourAngle = (now.Hour % 12) * 30 + minAngle / 12;

                SecondHand.RenderTransform = new RotateTransform(secAngle, 70, 70);
                MinuteHand.RenderTransform = new RotateTransform(minAngle, 70, 70);
                HourHand.RenderTransform = new RotateTransform(hourAngle, 70, 70);
            };
            _clockTimer.Start();
        }

        // Top Agent Modal
        private void Agent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string agentName)
            {
                var recentDeals = new List<Deal>
                {
                    new Deal { Property = "House A", Price = "$20,000", Date = "2025-09-01" },
                    new Deal { Property = "Condo B", Price = "$15,000", Date = "2025-09-05" },
                    new Deal { Property = "Lot C", Price = "$10,000", Date = "2025-09-10" }
                };

                var modal = new TopAgentModal();
                modal.SetAgentData(agentName, 25, "$50,000", 12, recentDeals);

                var borderWrapper = new Border
                {
                    CornerRadius = new CornerRadius(15),
                    Background = Brushes.White,
                    Child = modal,
                    Padding = new Thickness(0),
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        BlurRadius = 10,
                        Opacity = 0.2,
                        ShadowDepth = 2
                    },
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var overlay = new Grid
                {
                    Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0))
                };
                overlay.Children.Add(borderWrapper);
                MainGrid.Children.Add(overlay);

                modal.CloseClicked += () => MainGrid.Children.Remove(overlay);
            }
        }
    }

    // Converter for KPI bar height
    public class ProgressToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            double progress = System.Convert.ToDouble(value);
            double maxHeight = 80;
            return progress * maxHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}