using DocumentFlow.Models;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlow.ViewModels
{
    public class SchedulePageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;

        private DateTime? selectedDate;
        public DateTime? SelectedDate { get => selectedDate; set => Set(ref selectedDate, value); }
        private DateTime currentDate;
        public DateTime CurrentDate { get => currentDate; set => Set(ref currentDate, value); }

        private ObservableCollection<Event> eventList;
        public ObservableCollection<Event> EventList { get => eventList; set => Set(ref eventList, value); }

        private ObservableCollection<Event> eventListMonday;
        public ObservableCollection<Event> EventListMonday { get => eventListMonday; set => Set(ref eventListMonday, value); }
        private ObservableCollection<Event> eventListTuesday;
        public ObservableCollection<Event> EventListTuesday { get => eventListTuesday; set => Set(ref eventListTuesday, value); }
        private ObservableCollection<Event> eventListWednesday;
        public ObservableCollection<Event> EventListWednesday { get => eventListWednesday; set => Set(ref eventListWednesday, value); }
        private ObservableCollection<Event> eventListThursday;
        public ObservableCollection<Event> EventListThursday { get => eventListThursday; set => Set(ref eventListThursday, value); }
        private ObservableCollection<Event> eventListFriday;
        public ObservableCollection<Event> EventListFriday { get => eventListFriday; set => Set(ref eventListFriday, value); }
        private ObservableCollection<Event> eventListSaturday;
        public ObservableCollection<Event> EventListSaturday { get => eventListSaturday; set => Set(ref eventListSaturday, value); }
        private ObservableCollection<Event> eventListSunday;
        public ObservableCollection<Event> EventListSunday { get => eventListSunday; set => Set(ref eventListSunday, value); }

        private CalendarService GoogleCalendarService;
        public SchedulePageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GoogleCalendarService = googleService.GetQuickstartService();
            CurrentDate = DateTime.Today;
            SelectedDate = (DateTime?)CurrentDate;
            var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
            if(DateTime.Today.DayOfWeek.ToString() == "Saturday")
            {
                EventListSaturday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-4);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListTuesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-5);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Sunday")
            {
                EventListSunday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-4);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-5);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListTuesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-6);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Friday")
            {
                EventListFriday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListTuesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-4);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Thursday")
            {
                EventListThursday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(2);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListTuesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Wednesday")
            {
                EventListWednesday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(3);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(4);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListTuesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Tuesday")
            {
                EventListTuesday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(4);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(5);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(-1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListMonday = new ObservableCollection<Event>(eventss.Items);
            }
            else if (DateTime.Today.DayOfWeek.ToString() == "Monday")
            {
                EventListMonday = new ObservableCollection<Event>(events.Items);

                CurrentDate = DateTime.Today.AddDays(5);
                SelectedDate = (DateTime?)CurrentDate;
                var eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSaturday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(6);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListSunday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(4);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListFriday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(3);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListThursday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(2);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                eventListWednesday = new ObservableCollection<Event>(eventss.Items);

                CurrentDate = DateTime.Today.AddDays(1);
                SelectedDate = (DateTime?)CurrentDate;
                eventss = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                EventListTuesday = new ObservableCollection<Event>(eventss.Items);
            }

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            GoogleCalendarService = googleService.GetQuickstartService();
            CurrentDate = DateTime.Today;
            SelectedDate = (DateTime?)CurrentDate;
            var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
            EventList = new ObservableCollection<Event>(events.Items);
        }

        private RelayCommand<SelectionChangedEventArgs> selectedDatesChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> SelectedDatesChangedCommand => selectedDatesChangedCommand ?? (selectedDatesChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
                    EventList.Clear();
                    SelectedDate = ((System.Windows.Controls.Calendar)param.Source).SelectedDate;
                    if (SelectedDate != null)
                    {
                        var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                        EventList = new ObservableCollection<Event>(events.Items);
                    }
                }
            ));

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
