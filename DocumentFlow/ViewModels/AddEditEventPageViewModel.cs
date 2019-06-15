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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocumentFlow.ViewModels
{
    public class AddEditEventPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;

        private string eventSummary;
        public string EventSummary { get => eventSummary; set => Set(ref eventSummary, value); }
        private string location;
        public string Location { get => location; set => Set(ref location, value); }
        private string description;
        public string Description { get => description; set => Set(ref description, value); }
        private DateTime? startDate;
        public DateTime? StartDate { get => startDate; set => Set(ref startDate, value); }
        private DateTime? endDate;
        public DateTime? EndDate { get => endDate; set => Set(ref endDate, value); }
        private int colorIndex;
        public int ColorIndex { get => colorIndex; set => Set(ref colorIndex, value); }

        private ObservableCollection<EventAttendee> attendeesList;
        public ObservableCollection<EventAttendee> AttendeesList { get => attendeesList; set => Set(ref attendeesList, value); }
        private string freq;
        public string Freq { get => freq; set => Set(ref freq, value); }
        private int interval;
        public int Interval { get => interval; set => Set(ref interval, value); }
        private int count;
        public int Count { get => count; set => Set(ref count, value); }
        private bool hasRemainder;
        public bool HasRemainder { get => hasRemainder; set => Set(ref hasRemainder, value); }
        private int minutes;
        public int Minutes { get => minutes; set => Set(ref minutes, value); }
        private bool AddNew { get; set; }
        private Event SelectedEvent { get; set; }
        private CalendarService GoogleCalendarService;


        private User CurrentUser { get; set; }

        private string fio;
        public string Fio { get => fio; set => Set(ref fio, value); }

        private string avatara;
        public string Avatara { get => avatara; set => Set(ref avatara, value); }

        public AddEditEventPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;

            colorIndex = 1;

            Messenger.Default.Register<NotificationMessage<Event>>(this, OnHitIt);
            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);

            Messenger.Default.Register<NotificationMessage<CalendarService>>(this, goo =>
            {
                GoogleCalendarService = goo.Content;
            });
        }

        

        private void OnHitIt(NotificationMessage<Event> edev)
        {
            if (edev.Notification == "EventToEdit")
            {
                AddNew = false;
                var ev = edev.Content;
                SelectedEvent = edev.Content;
                EventSummary = ev.Summary;
                Location = ev.Location;
                Description = ev.Description;

                if (SelectedEvent.ColorId != null)
                    ColorIndex = int.Parse(SelectedEvent.ColorId) - 1;

                if (ev.Start.DateTime == null)
                {
                    StartDate = DateTime.Parse(ev.Start.Date, new CultureInfo("en-US", true));
                }
                else
                    StartDate = ev.Start.DateTime;

                if (ev.End.DateTime == null)
                {
                    EndDate = DateTime.Parse(ev.End.Date, new CultureInfo("en-US", true));
                }
                else
                    EndDate = ev.End.DateTime;
                
                if (ev.Attendees != null)
                    AttendeesList = new ObservableCollection<EventAttendee>(ev.Attendees);
            }
            else if (edev.Notification == "EventToAdd")
            {
                AddNew = true;
                SelectedEvent = edev.Content;
                EventSummary = SelectedEvent.Summary;
                Location = SelectedEvent.Location;
                Description = SelectedEvent.Description;
                //ColorIndex = int.Parse(SelectedEvent.ColorId) - 1;
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
                AttendeesList = new ObservableCollection<EventAttendee>();
            }

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


    

        #region NavigationCommands

        private RelayCommand saveEvent;
        public RelayCommand SaveEvent => saveEvent ?? (saveEvent = new RelayCommand(
                () =>
                {
                    var CurrentUserEmail = CurrentUser.GoogleAccount;

                    SelectedEvent.Summary = EventSummary;
                    SelectedEvent.Location = Location;
                    SelectedEvent.Description = Description;
                    SelectedEvent.ColorId = (ColorIndex + 1).ToString();
                    SelectedEvent.Start = new EventDateTime()
                    {
                        DateTime = StartDate,
                        TimeZone = "Asia/Baku",
                    };
                    SelectedEvent.End = new EventDateTime()
                    {
                        DateTime = EndDate,
                        TimeZone = "Asia/Baku",
                    };
                    if (Freq == "1")
                    {
                        Freq = "WEEKLY";
                    }else if (Freq == "0")
                    {
                        Freq = "DAILY";
                    }
                    if (string.IsNullOrEmpty(Freq))
                    {
                        Freq = "DAILY";
                    }
                    if (Count == 0)
                    {
                        Count = 1;
                    }
                    if (Interval == 0)
                    {
                        Interval = 1;
                    }
                    SelectedEvent.Recurrence = new string[] { $"RRULE:FREQ={Freq};INTERVAL={Interval};COUNT={Count}" };
                    SelectedEvent.Attendees = AttendeesList;
                    

                    if (HasRemainder)
                    {
                        SelectedEvent.Reminders = new Event.RemindersData()
                        {
                            UseDefault = false,
                            Overrides = new EventReminder[] {
                                new EventReminder() { Method = "email", Minutes = Minutes },
                            }
                        };
                    }

                    if (AddNew)
                    {
                        SelectedEvent.Attendees.Add(new EventAttendee
                        {
                            Email = CurrentUserEmail,
                            DisplayName = "me",
                            ResponseStatus = "accepted",
                            Self = true
                        });
                        SelectedEvent.Kind = "calendar#event";
                        googleService.addNewEvent(GoogleCalendarService, SelectedEvent);
                    }
                    else
                    {
                        googleService.updateEvent(GoogleCalendarService, SelectedEvent);
                    }
                    navigationService.Navigate<CalendarPageView>();
                }
                 ));
        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<CalendarPageView>();
                }
                 ));
        #endregion
    }
}