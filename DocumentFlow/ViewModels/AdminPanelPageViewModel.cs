using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        private RelayCommand<User> changeUserPasswordCommand;
        public RelayCommand<User> ChangeUserPasswordCommand => changeUserPasswordCommand ?? (changeUserPasswordCommand = new RelayCommand<User>(
                param =>
                {
                    
                }
                 ));

        private RelayCommand addNewUserCommand;
        public RelayCommand AddNewUserCommand => addNewUserCommand ?? (addNewUserCommand = new RelayCommand(
                () =>
                {

                }
                 ));

        private RelayCommand changeMyPassword;
        public RelayCommand ChangeMyPassword => changeMyPassword ?? (changeMyPassword = new RelayCommand(
                () =>
                {

                }
                 ));

       
    }
}
