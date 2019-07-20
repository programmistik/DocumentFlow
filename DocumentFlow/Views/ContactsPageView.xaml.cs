using DocumentFlow.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocumentFlow.Views
{
    /// <summary>
    /// Interaction logic for ContactsPageView.xaml
    /// </summary>
    public partial class ContactsPageView : UserControl
    {
        public ContactsPageView()
        {
            InitializeComponent();
        }


    }


    public class OrgCompany : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is Employee)
                {
                    var val = value as Employee;
                    return val.Company.CompanyName;
                }
                else if (value is ExternalContact)
                {
                    var val = value as ExternalContact;
                    return val.Organization.OrganizationName;
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // never used
            return null;
        }

    }

    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0];
            var prop = values[1];

            if (item != null)
            {
                if (item is Employee)
                {
                    return false;
                }
                else if (item is ExternalContact)
                {
                   if (prop != null)
                    {
                        var val = (bool)prop;
                        return val;
                    }
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
