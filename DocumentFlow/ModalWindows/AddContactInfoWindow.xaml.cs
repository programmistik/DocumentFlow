using DocumentFlow.Models;
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

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c < '0' || c > '9') && c != '+' )
                    return false;
            }
            return true;
        }

        bool EmailValidation(string str)
        {
            string email = str;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {            

            if (InfoCollection.SelectedIndex.ToString() == "0" || InfoCollection.SelectedIndex.ToString() == "1")
            {
                if (IsDigitsOnly(InputValue.Text) == false)
                {
                    MessageBox.Show("Invalid phone");
                }
                else
                {
                    Hide();
                }

            }
            if (InfoCollection.SelectedIndex.ToString() == "3")
            {
                if (EmailValidation(InputValue.Text) == false)
                {
                    MessageBox.Show("Invalid e-mail format");
                }
                else
                {
                    Hide();
                }

            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue.Text = null;
            Hide();
        }

        private void InfoCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InputValue.Text = null;
        }
    }
}
