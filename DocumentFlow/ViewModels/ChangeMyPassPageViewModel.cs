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
    public class ChangeMyPassPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User user;
        public User CurrentUser { get => user; set => Set(ref user, value); }

        public ChangeMyPassPageViewModel(INavigationService navigationService,
                                             IMessageService messageService,
                                                AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, usr =>
            {
                CurrentUser = usr.Content;
            });
        }
    }
}