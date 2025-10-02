using System.Windows;
using Real_Estate_Agencies.View;  // ✅ Add this

namespace Real_Estate_Agencies
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ✅ This matches your LoginView.xaml.cs
            var loginWindow = new LoginView();
            loginWindow.Show();
        }
    }
}
