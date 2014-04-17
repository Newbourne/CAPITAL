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

namespace CAPITAL.ViewModels
{
    public class AboutViewModel
    {
        public string Name
        {
            get
            {
                return "Brian Bland";
            }
        }

        public string Residence
        {
            get
            {
                return "Florence, KY";
            }
        }

        public string Message
        {
            get
            {
                return "I enjoy using the latest Microsoft technologies (Windows Phone, C#, .NET, Entity Framework and WCF). I am always looking for new opportunities. Please contact me through the feedback section if you wish to inquire about .NET opportunities.";
            }
        }
    }
}
