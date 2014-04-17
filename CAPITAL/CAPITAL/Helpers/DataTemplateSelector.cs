using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;
using CAPITAL.CapitalService;

namespace CAPITAL.Helpers
{
    public class DataTemplateSelector : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (((Account)newContent).IsShare == true)
            {
                ContentTemplate = DataTemplateSelector.LoadFromDictionary(
                                    "CAPITAL;component/Styles/DataTemplates.xaml",
                                    "AccountSharedTemplate");
            }
            else
            {
                ContentTemplate = DataTemplateSelector.LoadFromDictionary(
                                    "CAPITAL;component/Styles/DataTemplates.xaml",
                                    "AccountOwnerTemplate");
            }
        }

        public static DataTemplate LoadFromDictionary(string dictionary, string template)
        {
            var doc = XDocument.Load(dictionary);
            var dict = (ResourceDictionary)XamlReader.Load(doc.ToString(SaveOptions.None));
            return dict[template] as DataTemplate;
        }
    }
}