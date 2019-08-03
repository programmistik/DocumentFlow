using BespokeFusion;
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

namespace DocumentFlow.Views
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow()
        {
            InitializeComponent();
        }

        private void ImageBrush_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void TheMainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                var msg = new CustomMaterialMessageBox
                {
                    TxtMessage = { Text = "Are you sure you want to quit?", Foreground = Brushes.Black },
                    TxtTitle = { Text = "Exit", Foreground = Brushes.White },
                    BtnOk = { Content = "Yes" },
                    BtnCancel = { Content = "No" },
                    MainContentControl = { Background = Brushes.White },
                    TitleBackgroundPanel = { Background = Brushes.BlueViolet },

                    BorderBrush = Brushes.BlueViolet
                };

                msg.Show();
                var results = msg.Result;
                if (results.ToString() == "OK")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            
        }
    }
}
