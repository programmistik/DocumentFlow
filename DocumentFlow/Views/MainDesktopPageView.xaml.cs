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
    /// Interaction logic for MainDesktopPageView.xaml
    /// </summary>
    public partial class MainDesktopPageView : UserControl
    {
        public MainDesktopPageView()
        {
            InitializeComponent();
        }
        private void MenuClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            MessageBox.Show(button.Content.ToString(), "HexMenu", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void HexItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }



}
