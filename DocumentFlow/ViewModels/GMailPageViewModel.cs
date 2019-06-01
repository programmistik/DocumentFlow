using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DocumentFlow.ViewModels
{
    public class GMailPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;
        private GmailService GMailService;

        private ObservableCollection<GoogleMessage> inboxList;
        public ObservableCollection<GoogleMessage> InboxList { get => inboxList; set => Set(ref inboxList, value); }

        private User CurrentUser { get; set; }

        private string fio;
        public string Fio { get => fio; set => Set(ref fio, value); }

        private string avatara;
        public string Avatara { get => avatara; set => Set(ref avatara, value); }

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


        private string filter;
        public string MailFilter
        {
            get { return filter; }
            set
            {
                filter = value;
                RaisePropertyChanged("FilteredInboxItems");
            }
        }
        public ObservableCollection<GoogleMessage> FilteredInboxItems
        {
            get
            {
                if (string.IsNullOrEmpty(MailFilter))
                    return InboxList;
                var col = InboxList.Where(msg => msg.Subject.ToLower().Contains(MailFilter.ToLower())).ToList();
                return new ObservableCollection<GoogleMessage>(col);
            }
        }


        public GMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GMailService = googleService.getGMailService();
            InboxList = new ObservableCollection<GoogleMessage>();

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);

        }


        private RelayCommand receiveMailCommand;
        public RelayCommand ReceiveMailCommand => receiveMailCommand ?? (receiveMailCommand = new RelayCommand(
                async () =>
                {
                    UsersResource.LabelsResource.ListRequest request = GMailService.Users.Labels.List("me");

                    // List labels.
                    IList<Label> labels = request.Execute().Labels;

                    var inboxlistRequest = GMailService.Users.Messages.List("me");
                    inboxlistRequest.LabelIds = "INBOX";
                    inboxlistRequest.LabelIds = "CATEGORY_PERSONAL";
                    inboxlistRequest.IncludeSpamTrash = false;
                    //get our emails
                    var emailListResponse = inboxlistRequest.Execute();

                    if (emailListResponse != null && emailListResponse.Messages != null)
                    {
                        //loop through each email and get what fields you want...
                        foreach (var email in emailListResponse.Messages)
                        {
                            var newMail = await googleService.getMessageAsync(email, GMailService);
                            InboxList.Add(newMail);

                        }

                    }
                }
                 ));

        private RelayCommand<GoogleMessage> readMailCommand;
        public RelayCommand<GoogleMessage> ReadMailCommand => readMailCommand ?? (readMailCommand = new RelayCommand<GoogleMessage>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<GoogleMessage>(param, "MailToRead"));
                    navigationService.Navigate<ReadMailPageView>();
                }
                 ));
        private RelayCommand composeCommand;
        public RelayCommand ComposeCommand => composeCommand ?? (composeCommand = new RelayCommand(
                () =>
                {
                    //var mess = new Message();
                    Messenger.Default.Send(new NotificationMessage<GmailService>(GMailService, "GService"));
                    navigationService.Navigate<ComposeNewMailPageView>();
                }
                 ));

        #region NavigationCommands
        //Upper Menu
        private RelayCommand gMain;
        public RelayCommand GMain => gMain ?? (gMain = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));

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
