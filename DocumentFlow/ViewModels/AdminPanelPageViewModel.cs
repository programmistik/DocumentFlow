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

        public AdminPanelPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            UserList = new ObservableCollection<User>(db.Users);
            var admin = UserList.Where(u => u.Login == "admin").Single();
            UserList.Remove(admin);
        }

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

                }
                 ));
        private RelayCommand normInfoCommand;
        public RelayCommand NormInfoCommand => normInfoCommand ?? (normInfoCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<NormativeInfoPageView>();
                }
                 ));

        private RelayCommand addNewsCommand;
        public RelayCommand AddNewsCommand => addNewsCommand ?? (addNewsCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AddNewsPageView>();
                }
                 ));


    }
}
