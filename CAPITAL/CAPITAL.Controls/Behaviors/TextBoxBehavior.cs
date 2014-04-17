using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace CAPITAL.Controls.Behaviors
{
    public sealed class TextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += new System.Windows.RoutedEventHandler(TextBoxBehaviorGotFocus);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= TextBoxBehaviorGotFocus;
        }

        private void TextBoxBehaviorGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Foreground = Application.Current.Resources["AppBackground"] as Brush;
            AssociatedObject.SelectionBackground = Application.Current.Resources["AppAccentColor1"] as Brush;
            AssociatedObject.Background = Application.Current.Resources["AppAccentColor1"] as Brush;
            AssociatedObject.BorderThickness = new Thickness(3);
            AssociatedObject.BorderBrush = Application.Current.Resources["AppAccentColor1"] as Brush;
        }
    }
}
