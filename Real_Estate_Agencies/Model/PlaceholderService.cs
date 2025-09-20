using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies.Helpers
{
    public static class PlaceholderService
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(PlaceholderService), new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                UpdatePlaceholder(textBox);
                textBox.GotFocus += (s, args) => UpdatePlaceholder(textBox);
                textBox.LostFocus += (s, args) => UpdatePlaceholder(textBox);
            }
        }

        private static void UpdatePlaceholder(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.SetCurrentValue(TextBlock.TextProperty, GetPlaceholder(textBox));
                // You would need to add logic to show/hide the placeholder
                // A common approach is to use a Border or TextBlock overlaid on the TextBox
            }
            else
            {
                // Placeholder should be hidden
            }
        }
    }
}