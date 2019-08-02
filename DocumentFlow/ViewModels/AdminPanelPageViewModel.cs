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
    public class AdminPanelPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private ObservableCollection<User> userList;
        public ObservableCollection<User> UserList { get => userList; set => Set(ref userList, value); }

        private bool showActiveOnly;
        public bool ShowActiveOnly { get => showActiveOnly; set => Set(ref showActiveOnly, value); }

        public AdminPanelPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            ShowActiveOnly = true;
            UserList = new ObservableCollection<User>(db.Users.Where(u => u.IsActive == true));
            var admin = UserList.Where(u => u.Login == "admin").Single();
            UserList.Remove(admin);
        }

        private string filter;
        public string UserFilter
        {
            get { return filter; }
            set
            {
                filter = value;
                RaisePropertyChanged("FilteredUsers");
            }
        }
        public ObservableCollection<User> FilteredUsers
        {
            get
            {
                if (string.IsNullOrEmpty(UserFilter))
                    return UserList;
                var col = UserList.Where(usr => usr.ToString().ToLower().Contains(UserFilter.ToLower())).ToList();
                return new ObservableCollection<User>(col);
            }
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            if (ShowActiveOnly)
                UserList = new ObservableCollection<User>(db.Users.Where(u => u.IsActive == true));
            else
                UserList = new ObservableCollection<User>(db.Users);
            var admin = UserList.Where(u => u.Login == "admin").Single();
            UserList.Remove(admin);
            RaisePropertyChanged("FilteredUsers");

        }

        private RelayCommand showUsersCommand;
        public RelayCommand ShowUsersCommand => showUsersCommand ?? (showUsersCommand = new RelayCommand(
                () =>
                {
                    if (ShowActiveOnly)
                        UserList = new ObservableCollection<User>(db.Users.Where(u => u.IsActive == true));
                    else
                        UserList = new ObservableCollection<User>(db.Users);
                    var admin = UserList.Where(u => u.Login == "admin").Single();
                    UserList.Remove(admin);
                    RaisePropertyChanged("FilteredUsers");
                }
                 ));

        private RelayCommand<User> editUserCommand;
        public RelayCommand<User> EditUserCommand => editUserCommand ?? (editUserCommand = new RelayCommand<User>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(param, "OpenToEdit"));
                    navigationService.Navigate<AddNewUserPageView>();
                }
                 ));

        private RelayCommand addNewUserCommand;
        public RelayCommand AddNewUserCommand => addNewUserCommand ?? (addNewUserCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(new User(), "OpenToAdd"));
                    navigationService.Navigate<AddNewUserPageView>();

                }
                 ));

        private RelayCommand changeMyPassword;
        public RelayCommand ChangeMyPassword => changeMyPassword ?? (changeMyPassword = new RelayCommand(
                () =>
                {
                    var admin = db.Users.Where(usr => usr.Login == "admin").Single();
                    Messenger.Default.Send(new NotificationMessage<User>(admin, "change"));
                    navigationService.Navigate<ChangeMyPassPageView>();
                }
                 ));
        private RelayCommand normInfoCommand;
        public RelayCommand NormInfoCommand => normInfoCommand ?? (normInfoCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<NormativeInfoPageView>();
                }
                 ));

        private RelayCommand newsCommand;
        public RelayCommand NewsCommand => newsCommand ?? (newsCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<NewsListPageView>();
                }
                 ));

        private RelayCommand constCommand;
        public RelayCommand ConstCommand => constCommand ?? (constCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<ConstantsPageView>();
                }
                 ));

        private RelayCommand logOffCommand;
        public RelayCommand LogOffCommand => logOffCommand ?? (logOffCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<LogInPageView>();
                }
                 ));

        private RelayCommand historyCommand;
        public RelayCommand HistoryCommand => historyCommand ?? (historyCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<ShowHistoryPageView>();
                }
                 ));
    }
}
