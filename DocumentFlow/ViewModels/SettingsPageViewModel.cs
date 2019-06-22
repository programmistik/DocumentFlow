using AForge.Video;
using AForge.Video.DirectShow;
using DocumentFlow.ModalWindows;
using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
    public class SettingsPageViewModel : ViewModelBase, IDisposable
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        

        private string ImageLink { get; set; }
        private BitmapImage image;
        public BitmapImage Image { get => image; set => Set(ref image, value); }
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        private FilterInfo currentDevice;
        public FilterInfo CurrentDevice { get => currentDevice; set => Set(ref currentDevice, value); }
        public IVideoSource videoSource;

        private string fio;
        public string Fio { get => fio; set => Set(ref fio, value); }

        //private string msgColor;
        //public string MsgColor { get => msgColor; set => Set(ref msgColor, value); }

        //private string passCheckError;
        //public string PassCheckError { get => passCheckError; set => Set(ref passCheckError, value); }

        //private bool passwordConfirmation;
        //public bool PasswordConfirmation { get => passwordConfirmation; set => Set(ref passwordConfirmation, value); }

        private User CurrentUser { get; set; }
        private Employee CurrentEmployee { get; set; }

        private string avatara;
        public string Avatara { get => avatara; set => Set(ref avatara, value); }

        private string googleAccount;
        public string GoogleAccount { get => googleAccount; set => Set(ref googleAccount, value); }

        private Language selectedLanguage;
        public Language SelectedLanguage { get => selectedLanguage; set => Set(ref selectedLanguage, value); }

        private ObservableCollection<Language> languageCollection;
        public ObservableCollection<Language> LanguageCollection { get => languageCollection; set => Set(ref languageCollection, value); }

        private ColorScheme selectedColor;
        public ColorScheme SelectedColor { get => selectedColor; set => Set(ref selectedColor, value); }

        private ObservableCollection<ColorScheme> colorCollection;
        public ObservableCollection<ColorScheme> ColorCollection { get => colorCollection; set => Set(ref colorCollection, value); }

        private ObservableCollection<ContactInformation> infoList;
        public ObservableCollection<ContactInformation> InfoList { get => infoList; set => Set(ref infoList, value); }


        public SettingsPageViewModel(INavigationService navigationService, 
                                    IMessageService messageService, 
                                    AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;            

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitIt);
            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);

            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();

        }

        private void OnHitUser(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUser = usr.Content;
                var emp = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                Fio = emp.Name + " " + emp.Surname;

                if (string.IsNullOrEmpty(emp.Photo))
                    Avatara = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Images\\user.png";
                else
                    Avatara = emp.Photo;
            }
        }

        private void OnHitIt(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUser = usr.Content;
                
                CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                LanguageCollection = new ObservableCollection<Language>(db.Languages);
                SelectedLanguage = CurrentEmployee.Language;
                ColorCollection = new ObservableCollection<ColorScheme>(db.ColorSchemes);
                SelectedColor = CurrentEmployee.ColorScheme;
                GoogleAccount = CurrentUser.GoogleAccount;
                InfoList = new ObservableCollection<ContactInformation>(CurrentEmployee.ContactInfos);
                Fio = CurrentEmployee.Name + " " + CurrentEmployee.Surname;

                if (string.IsNullOrEmpty(CurrentEmployee.Photo))
                    Avatara = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Images\\user.png";
                else
                    Avatara = CurrentEmployee.Photo;
            }
            else
            {
                InfoList = new ObservableCollection<ContactInformation>();
                GoogleAccount = "";
            }
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(
        () =>
        {            
            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();
            if (CurrentDevice != null)
            {
                videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                videoSource.NewFrame += video_NewFrame;
                videoSource.Start();
            }

        }));

        #region Functions for camera
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
        #endregion

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

                    if (!string.IsNullOrEmpty(ImageLink))
                    {
                        StopCamera();
                        Image = new BitmapImage(new Uri(ImageLink));
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
                   
                    //User = new User();
                    StopCamera();
                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                async() =>
                {
                    if (Image != null)
                    {
                        var oldPath = Image.UriSource.OriginalString;
                        var imgDir = db.Constants.FirstOrDefault().FilePath;

                        var newPath = imgDir + "\\" + Guid.NewGuid() + Path.GetExtension(oldPath);

                        try
                        {
                            File.Copy(oldPath, newPath, true);
                            CurrentEmployee.Photo = newPath;
                        }
                        catch (Exception ex)
                        {
                            messageService.ShowError(ex.Message);
                            throw;
                        }
                        
                        
                    }
                    await db.SaveChangesAsync();

                    StopCamera();
                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));
        }

        #region NavigationCommands
        //Upper Menu
        private RelayCommand gMain;
        public RelayCommand GMain => gMain ?? (gMain = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));

        private RelayCommand gExit;
        public RelayCommand GExit => gExit ?? (gExit = new RelayCommand(
                () =>
                {
                    StopCamera();
                    navigationService.Navigate<LogInPageView>();
                }
            ));


        //Aside

        private RelayCommand gSchedule;
        public RelayCommand GSchedule => gSchedule ?? (gSchedule = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<SchedulePageView>();
                }
            ));

        private RelayCommand gDocuments;
        public RelayCommand GDocuments => gDocuments ?? (gDocuments = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand gNews;
        public RelayCommand GNews => gNews ?? (gNews = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<NewsPageView>();
                }
            ));

        private RelayCommand gCalendar;
        public RelayCommand GCalendar => gCalendar ?? (gCalendar = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<CalendarPageView>();
                }
            ));

        private RelayCommand gMail;
        public RelayCommand GMail => gMail ?? (gMail = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<GMailPageView>();
                }
            ));

        private RelayCommand gContacts;
        public RelayCommand GContacts => gContacts ?? (gContacts = new RelayCommand(
                () =>
                {
                    StopCamera();
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "Contacts"));
                    navigationService.Navigate<ContactsPageView>();
                }
            ));


        #endregion

        private RelayCommand addCommand;
        public RelayCommand AddCommand => addCommand ?? (addCommand = new RelayCommand(
                () =>
                {
                    var lst = db.ContactInfoTypes.ToList();

                    var win = new AddContactInfoWindow(lst);

                    win.ShowDialog();
                    if (!string.IsNullOrEmpty(win.InputValue.Text))
                    {
                        var newInfo = new ContactInformation
                        {
                            Contact = CurrentEmployee,
                            ContactInfoType = win.InfoCollection.SelectedItem as ContactInfoType,
                            Value = win.InputValue.Text
                        };
                        InfoList.Add(newInfo);
                        CurrentEmployee.ContactInfos.Add(newInfo);
                    }

                }
            ));

        private RelayCommand changeMyPassword;
        public RelayCommand ChangeMyPassword => changeMyPassword ?? (changeMyPassword = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "change"));
                    navigationService.Navigate<ChangeMyPassPageView>();

                }
            ));
    }
}
