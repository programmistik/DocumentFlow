using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddEditEventPageView.xaml
    /// </summary>
    public partial class AddEditEventPageView : UserControl
    {
        public AddEditEventPageView()
        {
            InitializeComponent();
        }

        private void Interval_TextChanged(object sender, TextChangedEventArgs e)
        {
            Interval.Text = Regex.Replace(Interval.Text, "[^0-9]+", "");
        }

        private void Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            Time.Text = Regex.Replace(Time.Text, "[^0-9]+", "");

        }

        private void Count_TextChanged(object sender, TextChangedEventArgs e)
        {
            Count.Text = Regex.Replace(Count.Text, "[^0-9]+", "");

        }
    }
}
