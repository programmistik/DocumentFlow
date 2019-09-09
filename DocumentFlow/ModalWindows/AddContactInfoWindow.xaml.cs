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
        

        public AddContactInfoWindow(List<ContactInfoType> lst, ContactInfoType typ, string val)
        {
            InitializeComponent();
            DataContext = new AddContactInfoViewModel( lst,  typ,  val);
        }

    }
}
