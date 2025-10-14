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

        private SalesPage salesPageInstance;

        private void NavigateToSales()
        {
            if (salesPageInstance == null)
                salesPageInstance = new SalesPage();

            MainFrame.Content = salesPageInstance;
        }


        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new DashboardPage();
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

            if (clicked == BtnDashboard) AnimateFrameContent(new DashboardPage());
            else if (clicked == BtnClients) AnimateFrameContent(new ClientsPage());
            else if (clicked == BtnProperties) AnimateFrameContent(new PropertiesPage());
            else if (clicked == BtnAgents) AnimateFrameContent(new AgentsPage());
            else if (clicked == BtnSales) MainFrame.Content = new SalesPage();
            else if (clicked == BtnCommissions) AnimateFrameContent(new CommissionsPage());
            else if (clicked == BtnIncentives) AnimateFrameContent(new IncentivesPage());
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

        private void BtnLogout_Checked(object sender, RoutedEventArgs e)
        {
            // Open LoginView
            LoginView login = new LoginView();
            login.Show();

            // Close MainWindow
            this.Close();
        }

    }
}