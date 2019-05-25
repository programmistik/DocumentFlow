using GalaSoft.MvvmLight.CommandWpf;
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
    /// Interaction logic for AddOrganizationWindow.xaml
    /// </summary>
    public partial class AddOrganizationWindow : Window
    {
        public bool CreateNew { get; set; } = false;
 
        public AddOrganizationWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new RelayCommand(
                () =>
                {
                    Hide();
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    CreateNew = true;
                    Hide();
                }
            ));
        }
    }
}
