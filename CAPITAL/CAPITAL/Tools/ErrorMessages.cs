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

namespace CAPITAL.Tools
{
    public static class ErrorMessages
    {
        public static void ValidationError(string message)
        {
            MessageBox.Show(message, "C'mon!", MessageBoxButton.OK);
        }

        public static void FaultError(string message)
        {
            MessageBox.Show(message, "Doh!", MessageBoxButton.OK);
        }

        public static void UnexpectedError()
        {
            MessageBox.Show("Unexpected Error Occurred! Please try again later.", "Ouch!", MessageBoxButton.OK);
        }
    }
}
