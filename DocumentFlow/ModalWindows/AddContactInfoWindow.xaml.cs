using DocumentFlow.Models;
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
using System.Windows.Shapes;

namespace DocumentFlow.ModalWindows
{
    /// <summary>
    /// Interaction logic for AddContactInfoWindow.xaml
    /// </summary>
    public partial class AddContactInfoWindow : Window
    {
        public AddContactInfoWindow(List<ContactInfoType> lst)
        {
            InitializeComponent();
            InfoCollection.ItemsSource = lst;
            InfoCollection.SelectedIndex = 0;
        }

       

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void InfoCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
