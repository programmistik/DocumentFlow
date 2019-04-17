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
using System.Threading;
using System.Threading.Tasks;

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

        public AddEditEventPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;

            colorIndex = 1;

            Messenger.Default.Register<NotificationMessage<Event>>(this, OnHitIt);

            Messenger.Default.Register<CalendarService>(this, goo =>
            {
                GoogleCalendarService = goo;
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
                if(ev.Start.DateTime == null)
                {
                    StartDate = DateTime.Parse(ev.Start.Date, new CultureInfo("en-US", true));
                }
                else
                    StartDate = ev.Start.DateTime;
                EndDate = ev.End.DateTime;
                AttendeesList = new ObservableCollection<EventAttendee>(ev.Attendees);
            }
            else if (edev.Notification == "EventToAdd")
            {
                AddNew = true;
                SelectedEvent = edev.Content;
                EventSummary = SelectedEvent.Summary;
                Location = SelectedEvent.Location;
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
                AttendeesList = new ObservableCollection<EventAttendee>();
            }

        }

        private RelayCommand saveEvent;
        public RelayCommand SaveEvent => saveEvent ?? (saveEvent = new RelayCommand(
                () =>
                {
                    var CurrentUserEmail = "3565733@gmail.com";

                    SelectedEvent.Summary = EventSummary;
                    SelectedEvent.Location = Location;
                    SelectedEvent.Description = Description;
                    SelectedEvent.ColorId = (ColorIndex+1).ToString();
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
                        //UserCredential credential;
                        //string[] Scopes = {
                        //            CalendarService.Scope.Calendar,
                        //            CalendarService.Scope.CalendarReadonly
                        //        };

                        //using (var stream =
                        //    new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\credentials.json", FileMode.Open, FileAccess.ReadWrite))
                        //{
                        //    // The file token.json stores the user's access and refresh tokens, and is created
                        //    // automatically when the authorization flow completes for the first time.
                        //    string credPath = "token.json";
                        //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        //        GoogleClientSecrets.Load(stream).Secrets,
                        //        Scopes,
                        //        "user",
                        //        CancellationToken.None,
                        //        new FileDataStore(credPath, true)).Result;
                        //    // MessageBox.Show("Credential file saved to: " + credPath);
                        //}
                        //var service = new CalendarService(new BaseClientService.Initializer()
                        //{
                        //    HttpClientInitializer = credential,
                        //    ApplicationName = "Google Calendar API .NET Quickstart",
                        //});



                        //Event newEvent = new Event()
                        //{
                        //    Summary = EventSummary,
                        //    Location = Location,
                        //    Description = Description,
                        //    ColorId = "2",
                        //    Start = new EventDateTime()
                        //    {
                        //        DateTime = StartDate,
                        //        TimeZone = "Asia/Baku",
                        //    },
                        //    End = new EventDateTime()
                        //    {
                        //        DateTime = EndDate,
                        //        TimeZone = "America/Los_Angeles",
                        //    },
                        //    Recurrence = new string[] { "RRULE:FREQ=DAILY;COUNT=2" },
                        //    Attendees = new EventAttendee[] {
                        //        new EventAttendee() { Email = "programmistik@gmail.com" },
                        //        new EventAttendee() { Email = "programmistik@yahoo.com" },
                        //    },
                        //    Reminders = new Event.RemindersData()
                        //    {
                        //        UseDefault = false,
                        //        Overrides = new EventReminder[] {
                        //            new EventReminder() { Method = "email", Minutes = 24 * 60 },
                        //            new EventReminder() { Method = "sms", Minutes = 10 },
                        //        }
                        //    }
                        //};

                        //string calendarId = "primary";
                        //EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                        //Event createdEvent = request.Execute();

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
    }
}