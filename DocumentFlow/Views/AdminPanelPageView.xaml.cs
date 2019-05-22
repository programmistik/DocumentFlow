﻿using System;
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
    /// Interaction logic for AdminPanelPageView.xaml
    /// </summary>
    public partial class AdminPanelPageView : UserControl
    {
        public AdminPanelPageView()
        {
            InitializeComponent();
        }

       
    }

    public class IsActiveToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == "True")
                    return "Black";
            }
            return "DarkRed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // never used
            return null;
        }
    }
}
