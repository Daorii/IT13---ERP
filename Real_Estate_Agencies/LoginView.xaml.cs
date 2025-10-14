using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Real_Estate_Agencies.View
{
    public partial class LoginView : Window
    {
        private string[] images = new string[]
        {
            @"C:\Users\maria\Downloads\Join the Seifert Capital family and achieve your financial dreams_ 🌟🏡__#FinancialDreams #SeifertCapital #JoinOurTeam #InvestWithSeifert.jpg",
            @"C:\Users\maria\Downloads\Nimbus palm village yamuna expressway.jpg",
            @"C:\Users\maria\Downloads\download (5).jpg"
        };

        private int currentIndex = 0;
        private bool useFirstImage = true;
        private DispatcherTimer timer = new DispatcherTimer();
        private Random rnd = new Random();

        public LoginView()
        {
            InitializeComponent();
            SetImage(images[currentIndex], true);
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int nextIndex = (currentIndex + 1) % images.Length;
            var nextBitmap = LoadBitmap(images[nextIndex]);

            Image fadingIn = useFirstImage ? Image2 : Image1;
            Image fadingOut = useFirstImage ? Image1 : Image2;

            fadingIn.Source = nextBitmap;
            fadingIn.Opacity = 0;

            fadingIn.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
            fadingOut.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1)));

            var scaleTransform = ((TransformGroup)fadingIn.RenderTransform).Children[0] as ScaleTransform;
            var translateTransform = ((TransformGroup)fadingIn.RenderTransform).Children[1] as TranslateTransform;

            StartKenBurnsEffect(scaleTransform, translateTransform, fadingIn);

            currentIndex = nextIndex;
            useFirstImage = !useFirstImage;
        }

        private BitmapImage LoadBitmap(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(path, UriKind.Absolute);
            bitmap.EndInit();
            return bitmap;
        }

        private void SetImage(string path, bool immediate)
        {
            Image img = useFirstImage ? Image1 : Image2;
            img.Source = LoadBitmap(path);
            img.Opacity = immediate ? 1 : 0;

            var scaleTransform = ((TransformGroup)img.RenderTransform).Children[0] as ScaleTransform;
            var translateTransform = ((TransformGroup)img.RenderTransform).Children[1] as TranslateTransform;

            StartKenBurnsEffect(scaleTransform, translateTransform, img);
        }

        private void StartKenBurnsEffect(ScaleTransform scale, TranslateTransform translate, Image img)
        {
            double targetScale = 1.03 + rnd.NextDouble() * 0.09;
            double maxOffsetX = (img.ActualWidth * (targetScale - 1)) / 2;
            double maxOffsetY = (img.ActualHeight * (targetScale - 1)) / 2;

            double offsetX = rnd.NextDouble() * 2 * maxOffsetX - maxOffsetX;
            double offsetY = rnd.NextDouble() * 2 * maxOffsetY - maxOffsetY;

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(1, targetScale, TimeSpan.FromSeconds(5)));
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation(1, targetScale, TimeSpan.FromSeconds(5)));
            translate.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(0, offsetX, TimeSpan.FromSeconds(5)));
            translate.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(0, offsetY, TimeSpan.FromSeconds(5)));
        }

        // Window drag
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void btnClose_Click(object sender, RoutedEventArgs e) => this.Close();

        private bool isFullScreen = false;
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F5)
            {
                if (!isFullScreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                    isFullScreen = true;
                }
                else
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Normal;
                    isFullScreen = false;
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            if (username == "admin" && password == "admin123")
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


    }
}
