using AForge.Video.DirectShow;
using DocumentFlow.Services;
using DocumentFlow.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SettingsPageView.xaml
    /// </summary>
    public partial class SettingsPageView : UserControl, IPasswordSupplier
    {
        public SettingsPageView()
        {
            InitializeComponent();
        }

        public SecureString GetPassword { get => pBox.SecurePassword; }

        public bool ConfirmPassword()
        {
            return pBox.Password == pBox2.Password;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var data = DataContext as SettingsPageViewModel;
            data.VideoDevices = new ObservableCollection<FilterInfo>();
            data.GetVideoDevices();
            if (data.CurrentDevice != null)
            {
                data.videoSource = new VideoCaptureDevice(data.CurrentDevice.MonikerString);
                data.videoSource.NewFrame += data.video_NewFrame;
                data.videoSource.Start();
            }
        }
    }
}
