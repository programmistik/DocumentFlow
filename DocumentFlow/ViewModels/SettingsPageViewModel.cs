using AForge.Video;
using AForge.Video.DirectShow;
using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DocumentFlow.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase//, IDisposable
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IPasswordSupplier passwordService;

        private string ImageLink { get; set; }
        private BitmapImage image;
        public BitmapImage Image { get => image; set => Set(ref image, value); }
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        private FilterInfo currentDevice;
        public FilterInfo CurrentDevice { get => currentDevice; set => Set(ref currentDevice, value); }
        public IVideoSource videoSource;

        private string msg;
        public string Msg { get => msg; set => Set(ref msg, value); }

        private string msgColor;
        public string MsgColor { get => msgColor; set => Set(ref msgColor, value); }

        private string passCheckError;
        public string PassCheckError { get => passCheckError; set => Set(ref passCheckError, value); }

        private bool passwordConfirmation;
        public bool PasswordConfirmation { get => passwordConfirmation; set => Set(ref passwordConfirmation, value); }


        public SettingsPageViewModel(INavigationService navigationService, 
                                    IMessageService messageService, 
                                    AppDbContext db, 
                                    IPasswordSupplier passwordService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.passwordService = passwordService;

            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();

        }
        public void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {

                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    var bi = new BitmapImage();
                    bi.BeginInit();
                    var ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();

                    bi.Freeze();
                    Dispatcher.CurrentDispatcher.Invoke(() => Image = bi);
                }
            }
            catch (Exception)
            {
                StopCamera();
            }
        }
        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.NewFrame -= video_NewFrame;
            }
            Image = null;
        }

        public void GetVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in devices)
                VideoDevices.Add(device);

            if (VideoDevices.Any())
                CurrentDevice = VideoDevices[0];
        }
        public void Dispose()
        {
            if (videoSource != null && videoSource.IsRunning)
                videoSource.SignalToStop();


        }

        private RelayCommand snapCommand;
        public RelayCommand SnapCommand
        {
            get => snapCommand ?? (snapCommand = new RelayCommand(
                () =>
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(Image));
                    ImageLink = Environment.CurrentDirectory + "\\snap.png";
                    using (var fileStream = new FileStream(ImageLink, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                    StopCamera();
                    Image = new BitmapImage(new Uri(ImageLink));
                }
            ));
        }
        private RelayCommand uploadFileCommand;
        public RelayCommand UploadFileCommand
        {
            get => uploadFileCommand ?? (uploadFileCommand = new RelayCommand(
                () =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "All files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == true)
                    {
                        ImageLink = openFileDialog.FileName;
                    }

                    if (ImageLink != "")
                    {
                        StopCamera();
                        Image = new BitmapImage(new Uri(ImageLink));
                    }
                }
            ));
        }

        private RelayCommand<string> lostFocusCommand_tbUN;
        public RelayCommand<string> LostFocusCommand_tbUN
        {
            get => lostFocusCommand_tbUN ?? (lostFocusCommand_tbUN = new RelayCommand<string>(
                param =>
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        //var qwr = db.Users.Where(u => u.Login == param);
                        //if (qwr.Any() == true)
                        //{
                        //    MsgColor = "Red";
                        //    Msg = "This username is already taken";
                        //}
                        //else
                        //{
                        //    MsgColor = "Green";
                        //    Msg = "You can use this username";
                        //}
                    }
                }
            ));
        }

        private RelayCommand<object> lostFocusCommand_pBox;
        public RelayCommand<object> LostFocusCommand_pBox
        {
            get => lostFocusCommand_pBox ?? (lostFocusCommand_pBox = new RelayCommand<object>(
                param =>
                {
                    var passwordContainer = param as IPasswordSupplier;
                    if (passwordContainer != null)
                    {
                        var chk = passwordContainer.ConfirmPassword();
                        if (chk)
                        {
                            PassCheckError = "";
                            PasswordConfirmation = true;
                        }
                        else
                        {
                            PassCheckError = "Passwords don't match";
                            PasswordConfirmation = false;
                        }
                    }
                }
            ));
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new RelayCommand(
                () =>
                {
                    // do nothing
                    MsgColor = Msg = PassCheckError = "";
                    //User = new User();
                    StopCamera();
                    navigationService.Navigate<LogInPageView>();
                }
            ));
        }
        //Navigation
        //Upper Menu
        private RelayCommand gMain;
        public RelayCommand GMain => gMain ?? (gMain = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));

        private RelayCommand gSettings;
        public RelayCommand GSettings => gSettings ?? (gSettings = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<SettingsPageView>();
                }
            ));

        private RelayCommand gExit;
        public RelayCommand GExit => gExit ?? (gExit = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<LogInPageView>();
                }
            ));


        //Aside

        private RelayCommand gSchedule;
        public RelayCommand GSchedule => gSchedule ?? (gSchedule = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<SchedulePageView>();
                }
            ));

        private RelayCommand gDocuments;
        public RelayCommand GDocuments => gDocuments ?? (gDocuments = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand gNews;
        public RelayCommand GNews => gNews ?? (gNews = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<NewsPageView>();
                }
            ));

        private RelayCommand gCalendar;
        public RelayCommand GCalendar => gCalendar ?? (gCalendar = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<CalendarPageView>();
                }
            ));

        private RelayCommand gMail;
        public RelayCommand GMail => gMail ?? (gMail = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<GMailPageView>();
                }
            ));

        private RelayCommand gContacts;
        public RelayCommand GContacts => gContacts ?? (gContacts = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<ContactsPageView>();
                }
            ));
    }
}
