using DocumentFlow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for AddNewUserPageView.xaml
    /// </summary>
    public partial class AddNewUserPageView : UserControl, IPasswordSupplier
    {
        public AddNewUserPageView()
        {
            InitializeComponent();
        }

        public SecureString GetPassword { get => pBox.SecurePassword; }

        public bool ConfirmPassword()
        {
            return pBox.Password == pBox2.Password;
        }

        
    }
}
