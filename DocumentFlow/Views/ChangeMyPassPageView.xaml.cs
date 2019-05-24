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
    /// Interaction logic for ChangeMyPassPageView.xaml
    /// </summary>
    public partial class ChangeMyPassPageView : UserControl, IPasswordSupplier
    {
        public ChangeMyPassPageView()
        {
            InitializeComponent();
        }

        public SecureString GetPassword { get => pBox.SecurePassword; }
        public SecureString GetCurrentPassword { get => pBoxCurr.SecurePassword; }

        public bool ConfirmPassword()
        {
            return pBox.Password == pBox2.Password;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(pBox.Password);
        }
    }
}
