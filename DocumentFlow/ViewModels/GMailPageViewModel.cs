using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GMailService = googleService.getGMailService();
            InboxList = new ObservableCollection<GoogleMessage>();
           
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

        #region
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
        #endregion
    }
}
