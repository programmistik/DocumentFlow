using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFlow.Services;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;

namespace DocumentFlow.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private ViewModelBase currentPage;
        public ViewModelBase CurrentPage { get => currentPage; set => Set(ref currentPage, value); }

        private string applicationTitle;
        public string ApplicationTitle { get => applicationTitle; set => Set(ref applicationTitle, value); }


        private readonly INavigationService navigation;

        public AppViewModel(INavigationService navigation)
        {
            this.navigation = navigation;

            //Messenger.Default.Register<ViewModelBase>(this, viewModel => CurrentPage = viewModel);
            Messenger.Default.Register<ViewModelBase>(this, viewModel =>
            {
                CurrentPage = viewModel;
                var str = viewModel.ToString();
                if (viewModel is LogInPageViewModel)
                    ApplicationTitle = Properties.Resources.LogIn;
                else if (viewModel is AddEditEventPageViewModel)
                    ApplicationTitle = Properties.Resources.AddEditEvents; //"Add or Edit events";
                else if (viewModel is AddNewsPageViewModel)
                    ApplicationTitle = "Add or Edit News";
                else if (viewModel is AddNewUserPageViewModel)
                {
                    var vm = viewModel as AddNewUserPageViewModel;
                    if (vm.ButtonOkContent == Properties.Resources.Create)
                        ApplicationTitle = Properties.Resources.AddNewUser; //"Add new user";
                    else
                        ApplicationTitle = Properties.Resources.EditUser; //"Edit user";
                }
                else if (viewModel is AdminPanelPageViewModel)
                    ApplicationTitle = Properties.Resources.AdminPanel; //"Administrator's panel";
                else if (viewModel is CalendarPageViewModel)
                    ApplicationTitle = Properties.Resources.Calendar; //"Calendar";
                else if (viewModel is ChangeMyPassPageViewModel)
                {
                    var vm = viewModel as ChangeMyPassPageViewModel;
                    ApplicationTitle = Properties.Resources.ChangePasswordFor + " " + vm.CurrentUser.Login;
                }
                else if (viewModel is ComposeNewMailPageViewModel)
                    ApplicationTitle = Properties.Resources.NewMail; //"New Mail";
                else if (viewModel is ConstantsPageViewModel)
                    ApplicationTitle = Properties.Resources.AppConst; //"Application's constants";
                else if (viewModel is ContactsPageViewModel)
                    ApplicationTitle = Properties.Resources.Contacts; //"Contacts";
                else if (viewModel is CreateNewContactPageViewModel)
                {
                    var vm = viewModel as CreateNewContactPageViewModel;
                    if (vm.ButtonOkContent == Properties.Resources.Create)
                        ApplicationTitle = Properties.Resources.CreateNewContact; //"Create new contact";
                    else
                        ApplicationTitle = Properties.Resources.EditContact; //"Edit contact";
                }
                else if (viewModel is DocPageViewModel)
                    ApplicationTitle = Properties.Resources.Documents; //"Documents";
                else if (viewModel is DocumentsPageViewModel)
                    ApplicationTitle = Properties.Resources.Documents; //"Documents";
                else if (viewModel is GMailPageViewModel)
                    ApplicationTitle = Properties.Resources.Mail; //"Mail";
                else if (viewModel is MainDesktopPageViewModel)
                    ApplicationTitle = Properties.Resources.MainPage; //"Main Page";
                else if (viewModel is NewsListPageViewModel)
                    ApplicationTitle = "Add or Edit News";
                else if (viewModel is NewsPageViewModel)
                    ApplicationTitle = Properties.Resources.News; //"News";
                else if (viewModel is NormativeInfoPageViewModel)
                    ApplicationTitle = "Normative information";
                else if (viewModel is ReadMailPageViewModel)
                    ApplicationTitle = Properties.Resources.ReadMail; //"Read Mail";
                else if (viewModel is SchedulePageViewModel)
                    ApplicationTitle = Properties.Resources.Schedule; //"Schedule";
                else if (viewModel is SettingsPageViewModel)
                    ApplicationTitle = Properties.Resources.Settings; //"Settings";
            }
            );
        }

        private RelayCommand<Type> navigateCommand;
        public RelayCommand<Type> NavigateCommand
        {
            get => navigateCommand ?? (navigateCommand = new RelayCommand<Type>(
                param =>
                {
                    navigation.Navigate(param);
                }
            ));
        }
    }
}
