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

namespace DocumentFlow.ViewModels
{
    public class AddEditEventPageViewModel : ViewModelBase
    {

        private ObservableCollection<GoogleColors> colors;
        public ObservableCollection<GoogleColors> Colors { get => colors; set => Set(ref colors, value); }

        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

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
        private GoogleColors selectedColor;
        public GoogleColors SelectedColor { get => selectedColor; set => Set(ref selectedColor, value); }

        private bool AddNew { get; set; }
        private GoogleEvent SelectedEvent { get; set; }

        public AddEditEventPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            Colors = CreateColorCollection();

            Messenger.Default.Register<NotificationMessage<GoogleEvent>>(this, OnHitIt);
        }

        private ObservableCollection<GoogleColors> CreateColorCollection()
        {
            var col = new ObservableCollection<GoogleColors>();
            col.Add(new GoogleColors("1", "Lavender", "#7986cb"));
            col.Add(new GoogleColors("2", "Sage", "#33b679"));
            col.Add(new GoogleColors("3", "Grape", "#8e24aa"));

            return col;
        }

        private void OnHitIt(NotificationMessage<GoogleEvent> edev)
        {
            if (edev.Notification == "EventToEdit")
            {
                AddNew = false;
                var ev = edev.Content;
                EventSummary = ev.EventSummary;
                Location = ev.Location;
                StartDate = ev.Start;
                EndDate = ev.End;
            }
            else if (edev.Notification == "EventToAdd")
            {
                AddNew = true;
                SelectedEvent = edev.Content;
                EventSummary = SelectedEvent.EventSummary;
                Location = SelectedEvent.Location;
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
            }

        }

        private RelayCommand saveEvent;
        public RelayCommand SaveEvent => saveEvent ?? (saveEvent = new RelayCommand(
                () =>
                {
                    if (AddNew)
                    {
                        UserCredential credential;
                        string[] Scopes = {
                                    CalendarService.Scope.Calendar,
                                    CalendarService.Scope.CalendarReadonly
                                };

                        using (var stream =
                            new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\credentials.json", FileMode.Open, FileAccess.ReadWrite))
                        {
                            // The file token.json stores the user's access and refresh tokens, and is created
                            // automatically when the authorization flow completes for the first time.
                            string credPath = "token.json";
                            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                GoogleClientSecrets.Load(stream).Secrets,
                                Scopes,
                                "user",
                                CancellationToken.None,
                                new FileDataStore(credPath, true)).Result;
                            // MessageBox.Show("Credential file saved to: " + credPath);
                        }
                        var service = new CalendarService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = "Google Calendar API .NET Quickstart",
                        });



                        Event newEvent = new Event()
                        {
                            Summary = EventSummary,
                            Location = Location,
                            Description = Description,
                            ColorId = "2",
                            Start = new EventDateTime()
                            {
                                DateTime = StartDate,
                                TimeZone = "America/Los_Angeles",
                            },
                            End = new EventDateTime()
                            {
                                DateTime = EndDate,
                                TimeZone = "America/Los_Angeles",
                            },
                            Recurrence = new string[] { "RRULE:FREQ=DAILY;COUNT=2" },
                            Attendees = new EventAttendee[] {
                                new EventAttendee() { Email = "programmistik@gmail.com" },
                                new EventAttendee() { Email = "programmistik@yahoo.com" },
                            },
                            Reminders = new Event.RemindersData()
                            {
                                UseDefault = false,
                                Overrides = new EventReminder[] {
                                    new EventReminder() { Method = "email", Minutes = 24 * 60 },
                                    new EventReminder() { Method = "sms", Minutes = 10 },
                                }
                            }
                        };

                        string calendarId = "primary";
                        EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                        Event createdEvent = request.Execute();
                        navigationService.Navigate<CalendarPageView>();
                    }
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