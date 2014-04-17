using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ImageTools;
using System.IO.IsolatedStorage;
using ImageTools.IO;
using ImageTools.IO.Png;

namespace CAPITAL.Shared.Tiles
{
    public partial class StatementTile : UserControl
    {
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(StatementTile), null);

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(DateTime?), typeof(StatementTile), null);

        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register("Account", typeof(string), typeof(StatementTile), null);

        public double Amount
        {
            get { return (double)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public string Account
        {
            get { return (string)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        public int Id
        {
            get;
            set;
        }

        public StatementTile()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Uri ToUri()
        {
            this.Measure(new Size(173, 173));
            this.Arrange(new Rect(0, 0, 173, 173));

            ExtendedImage tileImaged = this.ToImage();

            Encoders.AddEncoder<PngEncoder>();

            var encoder = new PngEncoder();

            var filename = string.Format("/Shared/ShellContent/StatementFront{0}.png", Id);

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.DirectoryExists("/Shared/ShellContent"))
                {
                    myIsolatedStorage.CreateDirectory("/Shared/ShellContent");
                }

                if (myIsolatedStorage.FileExists(filename))
                {
                    myIsolatedStorage.DeleteFile(filename);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename);
                encoder.Encode(tileImaged, fileStream);
                fileStream.Close();
            }

            return new Uri("isostore:" + filename, UriKind.RelativeOrAbsolute);
        }
    }
}
