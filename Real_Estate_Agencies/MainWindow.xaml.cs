using System;
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

        public MainWindow()
        {
            InitializeComponent();

            // Default page
            MainFrame.Content = new DashboardPage();
            BtnDashboard.IsChecked = true;
            ActiveButton = BtnDashboard;

            // Sidebar starts collapsed
            Sidebar.Width = 80;

            // Hide all labels at startup
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn.Content is StackPanel stack && stack.Children.Count > 1)
                {
                    var label = stack.Children[1] as TextBlock;
                    label.Opacity = 0;
                    label.Margin = new Thickness(-50, 0, 0, 0);
                }
            }

            // Attach hover events for sidebar
            Sidebar.MouseEnter += Sidebar_MouseEnter;
            Sidebar.MouseLeave += Sidebar_MouseLeave;
        }

        private void Sidebar_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateSidebarWidth(200);
            AnimateLabels(true, ignoreActive: false);
            AnimateIcons(true); // spin forward
        }

        private void Sidebar_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateSidebarWidth(80);
            AnimateLabels(false, ignoreActive: false);
            AnimateIcons(false); // spin back
        }

        private void AnimateSidebarWidth(double toWidth)
        {
            var anim = new DoubleAnimation
            {
                To = toWidth,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            Sidebar.BeginAnimation(WidthProperty, anim);
        }

        private void AnimateLabels(bool show, bool ignoreActive)
        {
            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn.Content is StackPanel stack && stack.Children.Count > 1)
                {
                    var label = stack.Children[1] as TextBlock;

                    if (ignoreActive && btn == ActiveButton)
                        continue;

                    label.BeginAnimation(OpacityProperty, new DoubleAnimation
                    {
                        To = show ? 1 : 0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                    });

                    label.BeginAnimation(MarginProperty, new ThicknessAnimation
                    {
                        To = show ? new Thickness(10, 0, 0, 0) : new Thickness(-50, 0, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(200),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                    });
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

                        double targetAngle = hover ? 360 : 0; // spin forward when hover, back when leave

                        var anim = new DoubleAnimation
                        {
                            To = targetAngle,
                            Duration = TimeSpan.FromMilliseconds(400),
                            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                        };

                        rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
                    }
                }
            }
        }

        private void Sidebar_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clicked) return;

            foreach (var child in SidebarPanel.Children)
            {
                if (child is ToggleButton btn && btn != clicked) btn.IsChecked = false;
            }

            ActiveButton = clicked;

            if (clicked == BtnDashboard) MainFrame.Content = new DashboardPage();
            else if (clicked == BtnClients) MainFrame.Content = new ClientsPage();
            else if (clicked == BtnProperties) MainFrame.Content = new PropertiesPage();
            else if (clicked == BtnAgents) MainFrame.Content = new AgentsPage();
            else if (clicked == BtnSales) MainFrame.Content = new SalesPage();
            else if (clicked == BtnCommissions) MainFrame.Content = new CommissionsPage();
            else if (clicked == BtnIncentives) MainFrame.Content = new IncentivesPage();
        }

        private void ShowLabel(ToggleButton btn, bool show)
        {
            if (btn.Content is StackPanel stack && stack.Children.Count > 1)
            {
                var label = stack.Children[1] as TextBlock;
                label.Opacity = show ? 1 : 0;
                label.Margin = show ? new Thickness(10, 0, 0, 0) : new Thickness(-50, 0, 0, 0);
            }
        }
    }
}
