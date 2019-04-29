using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        private string title;
        public string Title { get => title; set => Set(ref title, value); }
        private string from;
        public string From { get => from; set => Set(ref from, value); }
        private string mailDate;
        public string MailDate { get => mailDate; set => Set(ref mailDate, value); }

        public ReadMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<GoogleMessage>>(this, goo =>
            {
                MyMail = goo.Content;
                Title = "Subject: " + MyMail.Subject;
                From = "From: " + MyMail.From;
                MailDate = "Date: " + MyMail.Date;
            });
        }

        private RelayCommand OkCommand;
        public RelayCommand OKCommand => OkCommand ?? (OkCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<GMailPageView>();
                }
                 ));
    }

}
