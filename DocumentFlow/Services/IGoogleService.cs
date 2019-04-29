using DocumentFlow.Models;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Services
{
    public interface IGoogleService
    {
        CalendarService GetQuickstartService();
        Events GetEventsByDate(DateTime data, CalendarService service);
        void addNewEvent(CalendarService service, Event NewEvent, string calendarId = "primary");
        void updateEvent(CalendarService service, Event eventToUpdate, string calendarId = "primary");
        void deleteEvent(CalendarService service, Event eventToDelete, string calendarId = "primary");
        GmailService getGMailService();
        Task<GoogleMessage> getMessageAsync(Message email, GmailService GMailService);
        byte[] FromBase64ForUrlString(string base64ForUrlInput);
        string Base64UrlEncode(byte[] input);
        string CreateRawForNewMessage(string To, string Subject, string Text);

    }
}
