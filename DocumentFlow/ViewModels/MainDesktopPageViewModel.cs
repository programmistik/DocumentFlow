using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class MainDesktopPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        public MainDesktopPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

        }


        private RelayCommand<object> gCalendar;
        public RelayCommand<object> GCalendar
        {
            get => gCalendar ?? (gCalendar = new RelayCommand<object>(
                param =>
                {
                }
            ));
        }
    }
}
