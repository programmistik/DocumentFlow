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

        private string showEvents;
        public string ShowEvents { get => showEvents; set => Set(ref showEvents, value); }
        public CalendarPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            CurrentDate = DateTime.Now;
        }

        private RelayCommand<SelectionChangedEventArgs> selectedDatesChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> SelectedDatesChangedCommand => selectedDatesChangedCommand ?? (selectedDatesChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
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
                    MessageBox.Show("Upcoming events:");
                    if (events.Items != null && events.Items.Count > 0)
                    {
                        foreach (var eventItem in events.Items)
                        {
                            string when = eventItem.Start.DateTime.ToString();
                            if (String.IsNullOrEmpty(when))
                            {
                                when = eventItem.Start.Date;
                            }
                            MessageBox.Show($"{eventItem.Summary} ({when})");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No upcoming events found.");
                    }
                    //Console.Read();

                }
            ));
        private RelayCommand addEvent;
        public RelayCommand AddEvent => addEvent ?? (addEvent = new RelayCommand(
                () =>
                {
                    UserCredential credential;
                    string[] Scopes = { CalendarService.Scope.Calendar };

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

  //                  Event myEvent = new Event
  //                  {
  //                      Summary = "Appointment",
  //                      Location = "Somewhere",
  //                      Start = new EventDateTime()
  //                      {
  //                          DateTime = new DateTime(2019, 4, 12, 10, 0, 0),
  //                          TimeZone = "America/Los_Angeles"
  //                      },
  //                      End = new EventDateTime()
  //                      {
  //                          DateTime = new DateTime(2019, 4, 12, 10, 30, 0),
  //                          TimeZone = "America/Los_Angeles"
  //                      },
  //                      Recurrence = new String[] {
  //    "RRULE:FREQ=WEEKLY;BYDAY=MO"
  //},
  //                      Attendees = new List<EventAttendee>()
  //    {
  //      new EventAttendee() { Email = "3565733@gmail.com" }
  //    }
  //                  };

                Event ev = new Event();
                ev.Summary = "Google I/O 2015";
                ev.Location = "Baku";
                ev.Description = "A chance to hear more about Google's developer products.";

            DateTime startDateTime = DateTime.Now;
                EventDateTime start = new EventDateTime();
            start.DateTime = startDateTime;
            start.TimeZone = "America/Los_Angeles";
        ev.Start = start;

DateTime endDateTime = DateTime.Now;
                EventDateTime end = new EventDateTime();
            end.DateTime = endDateTime;
            end.TimeZone = "America/Los_Angeles";
        ev.End = end;

String[] recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" };
        ev.Recurrence = recurrence.ToList();

EventAttendee[] attendees = new EventAttendee[] {
    new EventAttendee(){ Email = "3565733@gmail.com" },
    new EventAttendee(){ Email = "programmistik@gmail.com" }
};
        ev.Attendees = attendees.ToList();

//EventReminder[] reminderOverrides = new EventReminder[] {
//    new EventReminder().setMethod("email").setMinutes(24 * 60),
//    new EventReminder().setMethod("popup").setMinutes(10),
//};
//        Event.Reminders reminders = new Event.Reminders()
//            .setUseDefault(false)
//            .setOverrides(Arrays.asList(reminderOverrides));
//        event.setReminders(reminders);

//String calendarId = "primary";
        //ev = service.events().insert(calendarId, event).execute();

                   Event recurringEvent = service.Events.Insert(ev, "primary").Execute();
                }
                 ));
    }
}