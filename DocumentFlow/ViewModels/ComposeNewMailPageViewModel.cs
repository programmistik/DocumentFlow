using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class ComposeNewMailPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        
        
        public ComposeNewMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

          //  Messenger.Default.Register<NotificationMessage<Message>>(this, OnHitIt);

        }

        //

        }

        
    }
//}
