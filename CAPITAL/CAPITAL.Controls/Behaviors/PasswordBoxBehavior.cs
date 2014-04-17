using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;

namespace CAPITAL.Controls.Behaviors
{
    public sealed class PasswordBoxBehavior : Behavior<PasswordBox>
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
