using DocumentFlow.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
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

        public CalendarService GetQuickstartService(User usr)
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
                string credPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Google\\" + usr.Login + "\\token.json";
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
            request.MaxResults = 30;
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
            if (string.IsNullOrEmpty(NewEvent.Location))
            {
                NewEvent.Location = "Баку, Азербайджан";
            }
            
            // "RRULE:FREQ=;INTERVAL=0;COUNT=0"
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

        //public int getEventCountForToday()
        //{
        //    var evs = GetEventsByDate(DateTime.Today, GetQuickstartService());
        //    return evs.Items.Count();
        //}

        public GmailService getGMailService(User usr)
        {
            string[] Scopes = {
                GmailService.Scope.GmailReadonly,
                GmailService.Scope.GmailSend
            };
            string ApplicationName = "Gmail API .NET Quickstart";

            UserCredential credential;

            using (var stream =
                new FileStream(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                //string credPath = "mail_token.json";
                
                string credPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Google\\"+usr.Login+"\\mail_token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public Task<GoogleMessage> getMessageAsync(Message email, GmailService GMailService)
        {
            return Task.Run(() =>
            {
                var newMail = new GoogleMessage();
                var emailInfoRequest = GMailService.Users.Messages.Get("me", email.Id);
                emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                var emailInfoResponse = emailInfoRequest.Execute();

                if (emailInfoResponse != null)
                {

                    newMail.FullMessage = emailInfoResponse;

                    var from = "";
                    var date = "";
                    var subject = "";

                    foreach (var mParts in emailInfoResponse.Payload.Headers)
                    {
                        if (mParts.Name == "Date")
                        {
                            date = mParts.Value;
                            newMail.Date = date;
                        }
                        else if (mParts.Name == "From")
                        {
                            from = mParts.Value;
                            newMail.From = from;
                        }
                        else if (mParts.Name == "Subject")
                        {
                            subject = mParts.Value;
                            newMail.Subject = subject;
                        }

                        if (date != "" && from != "")
                        {
                            if (emailInfoResponse.Payload.Parts != null)
                            {
                                foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                                {
                                    if (p.MimeType == "text/html")
                                    {
                                        byte[] data = FromBase64ForUrlString(p.Body.Data);
                                        string decodedString = Encoding.UTF8.GetString(data);
                                        newMail.Html = decodedString;
                                    }
                                }
                            }
                        }

                    }

                }
                return newMail;
            });
        }

        public byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(string.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }

        public string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        public string CreateRawForNewMessage(string To, string Subject, string Text)
        {
            string plainText = $"To: {To}\r\n" +
                       $"Subject: {Subject}\r\n" +
                       "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                       Text;

            return Base64UrlEncode(Encoding.UTF8.GetBytes(plainText));
            
        }
    }
}
