using DocumentFlow.Models;
using DocumentFlow.Properties;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlow.ViewModels
{
    public class MainDesktopPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User CurrentUser { get; set; }

        private string fio;
        public string Fio { get => fio; set => Set(ref fio, value); }

        private string avatara;
        public string Avatara { get => avatara; set => Set(ref avatara, value); }

        public MainDesktopPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitIt);
           

        }
        private void OnHitIt(NotificationMessage<User> usr)
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

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(
        () =>
        {
            var emp = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
            if (string.IsNullOrEmpty(emp.Photo))
                Avatara = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Images\\user.png";
            else
                Avatara = emp.Photo;

        }));


        private RelayCommand<StackPanel> menuClickCommand;
        public RelayCommand<StackPanel> MenuClickCommand => menuClickCommand ?? (menuClickCommand = new RelayCommand<StackPanel>(
        param =>
        {
            if (param.Name == Resources.News)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                navigationService.Navigate<NewsPageView>();
            }
            else if(param.Name == Resources.Mail)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                navigationService.Navigate<GMailPageView>();
            }
            else if (param.Name == Resources.Calendar)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                navigationService.Navigate<CalendarPageView>();
            }
            else if (param.Name == Resources.Schedule)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                navigationService.Navigate<SchedulePageView>();
            }
            else if (param.Name == Resources.Contacts)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "Contacts"));
                navigationService.Navigate<ContactsPageView>();
            }
            else if (param.Name == Resources.Documents)
            {
                Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                navigationService.Navigate<DocumentsPageView>();
            }

        }));

        #region NavigationCommands
        //Upper Menu

        private RelayCommand gSettings;
        public RelayCommand GSettings => gSettings ?? (gSettings = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
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
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<SchedulePageView>();
                }
            ));

        private RelayCommand gDocuments;
        public RelayCommand GDocuments => gDocuments ?? (gDocuments = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand gNews;
        public RelayCommand GNews => gNews ?? (gNews = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<NewsPageView>();
                }
            ));

        private RelayCommand gCalendar;
        public RelayCommand GCalendar => gCalendar ?? (gCalendar = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<CalendarPageView>();
                }
            ));

        private RelayCommand gMail;
        public RelayCommand GMail => gMail ?? (gMail = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<GMailPageView>();
                }
            ));

        private RelayCommand gContacts;
        public RelayCommand GContacts => gContacts ?? (gContacts = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "Contacts"));
                    navigationService.Navigate<ContactsPageView>();
                }
            ));


        #endregion

    }
}
