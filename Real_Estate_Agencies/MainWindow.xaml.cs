using Real_Estate_Agencies.View;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Real_Estate_Agencies
{
    public partial class MainWindow : Window
    {
        private ToggleButton ActiveButton = null;

        // Cache page instances to avoid recreating them
        private DashboardPage _dashboardPage;
        private ClientsPage _clientsPage;
        private PropertiesPage _propertiesPage;
        private AgentsPage _agentsPage;
        private SalesPage _salesPage;
        private CommissionsPage _commissionsPage;
        private IncentivesPage _incentivesPage;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize dashboard page only
            _dashboardPage = new DashboardPage();
            MainFrame.Content = _dashboardPage;
            BtnDashboard.IsChecked = true;
            ActiveButton = BtnDashboard;

            Sidebar.Width = 80;
            HideAllLabels();

            Sidebar.MouseEnter += Sidebar_MouseEnter;
            Sidebar.MouseLeave += Sidebar_MouseLeave;
        }

        private void Sidebar_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateSidebarWidth(200);
            AnimateLabels(true);
            AnimateIcons(true);
        }

        private void Sidebar_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateSidebarWidth(80);
            AnimateLabels(false);
            AnimateIcons(false);
        }

        private void AnimateSidebarWidth(double toWidth)
        {
            var anim = new DoubleAnimation(toWidth, TimeSpan.FromMilliseconds(200))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut } };
            Sidebar.BeginAnimation(WidthProperty, anim);
        }

        private void HideAllLabels()
        {
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn.Content is StackPanel stack && stack.Children.Count > 1)
                {
                    var label = stack.Children[1] as TextBlock;
                    label.Opacity = 0;
                    label.Margin = new Thickness(-40, 0, 0, 0);
                }
                else if (child is TextBlock tb) tb.Opacity = 0;
            }
        }

        private void AnimateLabels(bool show)
        {
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn.Content is StackPanel stack && stack.Children.Count > 1)
                {
                    var label = stack.Children[1] as TextBlock;
                    label.BeginAnimation(OpacityProperty, new DoubleAnimation(show ? 1 : 0, TimeSpan.FromMilliseconds(200)));
                    label.BeginAnimation(MarginProperty, new ThicknessAnimation(show ? new Thickness(10, 0, 0, 0) : new Thickness(-40, 0, 0, 0), TimeSpan.FromMilliseconds(200)));
                }
                else if (child is TextBlock tb)
                {
                    tb.BeginAnimation(OpacityProperty, new DoubleAnimation(show ? 1 : 0, TimeSpan.FromMilliseconds(200)));
                }
            }
        }

        private void AnimateIcons(bool hover)
        {
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn.Content is StackPanel stack && stack.Children.Count > 0)
                {
                    var icon = stack.Children[0] as TextBlock;
                    if (icon != null)
                    {
                        var rotate = new RotateTransform(0);
                        icon.RenderTransformOrigin = new Point(0.5, 0.5);
                        icon.RenderTransform = rotate;
                        double targetAngle = hover ? 360 : 0;
                        rotate.BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation(targetAngle, TimeSpan.FromMilliseconds(400)));
                    }
                }
            }
        }

        private void Sidebar_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clicked) return;

            foreach (var child in SidebarPanel.Children)
                if (child is ToggleButton btn && btn != clicked) btn.IsChecked = false;

            ActiveButton = clicked;

            if (clicked == BtnLogout)
            {
                MessageBox.Show("You have been logged out.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
                return;
            }

            // Database Settings button
            if (clicked == BtnDatabaseSettings)
            {
                var settingsWindow = new DatabaseSettingsWindow();
                settingsWindow.ShowDialog();
                return;
            }

            // Use cached instances - create only if null
            if (clicked == BtnDashboard)
            {
                if (_dashboardPage == null) _dashboardPage = new DashboardPage();
                AnimateFrameContent(_dashboardPage);
            }
            else if (clicked == BtnClients)
            {
                if (_clientsPage == null) _clientsPage = new ClientsPage();
                AnimateFrameContent(_clientsPage);
            }
            else if (clicked == BtnProperties)
            {
                if (_propertiesPage == null) _propertiesPage = new PropertiesPage();
                AnimateFrameContent(_propertiesPage);
            }
            else if (clicked == BtnAgents)
            {
                if (_agentsPage == null) _agentsPage = new AgentsPage();
                AnimateFrameContent(_agentsPage);
            }
            else if (clicked == BtnSales)
            {
                if (_salesPage == null) _salesPage = new SalesPage();
                AnimateFrameContent(_salesPage);
            }
            else if (clicked == BtnCommissions)
            {
                if (_commissionsPage == null) _commissionsPage = new CommissionsPage();
                AnimateFrameContent(_commissionsPage);
            }
            else if (clicked == BtnIncentives)
            {
                if (_incentivesPage == null) _incentivesPage = new IncentivesPage();
                AnimateFrameContent(_incentivesPage);
            }
        }

        private void AnimateFrameContent(UIElement newContent)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));

            fadeOut.Completed += (s, e) =>
            {
                MainFrame.Content = newContent;
                newContent.BeginAnimation(OpacityProperty, fadeIn);
            };

            if (MainFrame.Content is UIElement oldContent)
                oldContent.BeginAnimation(OpacityProperty, fadeOut);
            else
                MainFrame.Content = newContent;
        }
    }
}