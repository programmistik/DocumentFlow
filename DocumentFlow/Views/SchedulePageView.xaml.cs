using System;
using System.Collections.Generic;
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
    /// Interaction logic for SchedulePageView.xaml
    /// </summary>
    public partial class SchedulePageView : UserControl
    {
        public SchedulePageView()
        {
            InitializeComponent();
        }
    }
    public class IdToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == "1")
                    return "#7986cb";
                else if (value.ToString() == "2")
                    return "#33b679";
                else if (value.ToString() == "3")
                    return "#8e24aa";
                else if (value.ToString() == "4")
                    return "#e67c73";
                else if (value.ToString() == "5")
                    return "#f6c026";
                else if (value.ToString() == "6")
                    return "#f5511d";
                else if (value.ToString() == "7")
                    return "#039be5";
                else if (value.ToString() == "8")
                    return "#616161";
                else if (value.ToString() == "9")
                    return "#3f51b5";
                else if (value.ToString() == "10")
                    return "#0b8043";
                else if (value.ToString() == "11")
                    return "#d60000";
            }
            return "#049BDD";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // never used
            return null;
        }
    }
}
