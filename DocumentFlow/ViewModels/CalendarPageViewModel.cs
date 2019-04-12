using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DocumentFlow.Views;
using GalaSoft.MvvmLight.Messaging;

namespace DocumentFlow.ViewModels
{
    public class CalendarPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private DateTime? selectedDate;
        public DateTime? SelectedDate { get => selectedDate; set => Set(ref selectedDate, value); }
        private DateTime currentDate;
        public DateTime CurrentDate { get => currentDate; set => Set(ref currentDate, value); }

        private ObservableCollection<GoogleEvent> eventList;
        public ObservableCollection<GoogleEvent> EventList { get => eventList; set => Set(ref eventList, value); }

        public CalendarPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            CurrentDate = DateTime.Now;
            EventList = new ObservableCollection<GoogleEvent>();
        }

        private RelayCommand<SelectionChangedEventArgs> selectedDatesChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> SelectedDatesChangedCommand => selectedDatesChangedCommand ?? (selectedDatesChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
                    EventList.Clear();

                    SelectedDate = ((System.Windows.Controls.Calendar)param.Source).SelectedDate;
                    UserCredential credential;
                    string[] Scopes = { CalendarService.Scope.CalendarReadonly };

                    using (var stream =
                        new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\credentials.json", FileMode.Open, FileAccess.Read))
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

                    // Create Google Calendar API service.
                    var service = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google Calendar API .NET Quickstart",
                    });

                    // Define parameters of request.
                    EventsResource.ListRequest request = service.Events.List("primary");
                    request.TimeMin = SelectedDate;
                    if (SelectedDate != null)
                    {
                        var sd = (DateTime)SelectedDate;
                        request.TimeMax = sd.AddDays(1).AddTicks(-1);
                    }
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.MaxResults = 10;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    // List events.
                    Events events = request.Execute();
                 //   MessageBox.Show("Upcoming events:");
                    if (events.Items != null && events.Items.Count > 0)
                    {
                        foreach (var eventItem in events.Items)
                        {
                            string when = eventItem.Start.DateTime.ToString();
                            if (String.IsNullOrEmpty(when))
                            {
                                when = eventItem.Start.Date;
                            }
                         //   MessageBox.Show($"{eventItem.Summary} ({when})");
                            var newEvent = new GoogleEvent
                            {
                                Craeded = eventItem.Created,
                                CreatorEmail = eventItem.Creator.Email,
                                Description = eventItem.Description,
                                Location = eventItem.Location,
                                EventSummary = eventItem.Summary,
                                Updated = eventItem.Updated,
                                Start = eventItem.Start.DateTime,
                                End = eventItem.End.DateTime
                            };
                            EventList.Add(newEvent);
                        }
                    }
                    else
                    {
                      //  MessageBox.Show("No upcoming events found.");
                    }
                    //Console.Read();

                }
            ));
        private RelayCommand addEvent;
        public RelayCommand AddEvent => addEvent ?? (addEvent = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AddEditEventPageView>();
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
                    //    Summary = "Google I/O 2015",
                    //    Location = "800 Howard St., San Francisco, CA 94103",
                    //    Description = "A chance to hear more about Google's developer products.",
                    //    Start = new EventDateTime()
                    //    {
                    //        DateTime = DateTime.Parse("2019-04-10T09:00:00-07:00"),
                    //        TimeZone = "America/Los_Angeles",
                    //    },
                    //    End = new EventDateTime()
                    //    {
                    //        DateTime = DateTime.Parse("2019-04-10T17:00:00-07:00"),
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
                 ));

        private RelayCommand<GoogleEvent> deleteEventCommand;
        public RelayCommand<GoogleEvent> DeleteEventCommand => deleteEventCommand ?? (deleteEventCommand = new RelayCommand<GoogleEvent>(
                param =>
                {
                }
                 ));

        private RelayCommand<GoogleEvent> editEventCommand;
        public RelayCommand<GoogleEvent> EditEventCommand => editEventCommand ?? (editEventCommand = new RelayCommand<GoogleEvent>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<GoogleEvent>(param, "EventToEdit"));
                    navigationService.Navigate<AddEditEventPageView>();
                }
                 ));
    }


}