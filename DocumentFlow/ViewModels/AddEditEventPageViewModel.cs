using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class AddEditEventPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private string eventSummary;
        public string EventSummary { get => eventSummary; set => Set(ref eventSummary, value); }
        private string location;
        public string Location { get => location; set => Set(ref location, value); }
        private DateTime? startDate;
        public DateTime? StartDate { get => startDate; set => Set(ref startDate, value); }

        private DateTime? endDate;
        public DateTime? EndDate { get => endDate; set => Set(ref endDate, value); }

        private bool AddNew { get; set; }
        public AddEditEventPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<GoogleEvent>>(this, OnHitIt);
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
            
        }
    }
}
