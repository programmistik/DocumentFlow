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

namespace DocumentFlow.Services
{
    public class GoogleServices:IGoogleService
    {

        public CalendarService GetQuickstartService()
        {
            UserCredential credential;
            string[] Scopes = {
                                    CalendarService.Scope.Calendar,
                                    CalendarService.Scope.CalendarEvents
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
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Calendar API .NET Quickstart",
            });

        }

        public Events GetEventsByDate(DateTime data, CalendarService service)
        {          

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = data;
            if (data != null)
            {
                var sd = data;
                request.TimeMax = sd.AddDays(1).AddTicks(-1);
            }
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            return request.Execute();
        }

        public Events getAllEvents(CalendarService service, string calendarId = "primary")
        {
            // Define parameters of request.
           
            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = true;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            return events;
        }

        public void addNewEvent(CalendarService service, Event NewEvent, string calendarId = "primary")
        {
            service.Events.Insert(NewEvent, calendarId).Execute();
        }

        public void updateEvent(CalendarService service, Event eventToUpdate, string calendarId = "primary")
        {
            service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }

        public void deleteEvent(CalendarService service, Event eventToDelete, string calendarId = "primary")
        {
            service.Events.Delete(calendarId, eventToDelete.Id).Execute();
        }
    }
}
