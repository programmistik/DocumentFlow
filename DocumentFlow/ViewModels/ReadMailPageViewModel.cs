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
    public class ReadMailPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private GoogleMessage myMail;
        public GoogleMessage MyMail { get => myMail; set => Set(ref myMail, value); }

       

        public ReadMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<GoogleMessage>>(this, goo =>
            {
                MyMail = goo.Content;
            });
        }
    }

}
