using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class NewsListPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private ObservableCollection<NewsPost> newsList;
        public ObservableCollection<NewsPost> NewsList { get => newsList; set => Set(ref newsList, value); }

        private bool showActualOnly;
        public bool ShowActualOnly { get => showActualOnly; set => Set(ref showActualOnly, value); }

        public NewsListPageViewModel(INavigationService navigationService,
                                           IMessageService messageService,
                                              AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            if (ShowActualOnly)
            {
                NewsList = new ObservableCollection<NewsPost>(db.NewsPosts.Where(n => n.PostEndDate >= DateTime.Today));
            }
            else
                NewsList = new ObservableCollection<NewsPost>(db.NewsPosts);

        }
        private RelayCommand showActualOnlyCommand;
        public RelayCommand ShowActualOnlyCommand => showActualOnlyCommand ?? (showActualOnlyCommand = new RelayCommand(
                () =>
                {
                    if (ShowActualOnly)
                        NewsList = new ObservableCollection<NewsPost>(db.NewsPosts.Where(n => n.PostEndDate >= DateTime.Today));
                    else
                        NewsList = new ObservableCollection<NewsPost>(db.NewsPosts);
                }
                 ));

        private RelayCommand addNewsCommand;
        public RelayCommand AddNewsCommand => addNewsCommand ?? (addNewsCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AddNewsPageView>();
                }
                 ));

        private RelayCommand<NewsPost> editNewsCommand;
        public RelayCommand<NewsPost> EditNewsCommand => editNewsCommand ?? (editNewsCommand = new RelayCommand<NewsPost>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<NewsPost>(param, "EditPost"));
                    navigationService.Navigate<AddNewsPageView>();
                }
                 ));

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AdminPanelPageView>();
                }
                 ));
    }
}
