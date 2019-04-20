using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class CalendarQuickstart
    {
      
        public static CalendarService service { get; set; }

        public CalendarQuickstart()
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
            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Calendar API .NET Quickstart",
            });

        }
    }
}