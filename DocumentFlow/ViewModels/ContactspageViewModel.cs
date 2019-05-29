using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentFlow.ViewModels
{
    public class ContactsPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private ObservableCollection<Contact> contactsList;
        public ObservableCollection<Contact> ContactsList { get => contactsList; set => Set(ref contactsList, value); }

        private bool addIsEnabled;
        public bool AddIsEnabled { get => addIsEnabled; set => Set(ref addIsEnabled, value); }

        private Employee currentEmployee;
        public Employee CurrentEmployee { get => currentEmployee; set => Set(ref currentEmployee, value); }

        public ContactsPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            ContactsList = new ObservableCollection<Contact>(db.Contacts);
            ContactsList.OrderBy(c => c.Name);

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitIt);
        }

        private void OnHitIt(NotificationMessage<User> usr)
        {
            if (usr.Notification == "Contacts")
            {
               
                var CurrentUser = usr.Content;
                CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                AddIsEnabled = CurrentEmployee.CanEditContacts;
                RaisePropertyChanged("FilteredContacts");
            }
        }

        private string filter;
        public string ContactsFilter
        {
            get { return filter; }
            set
            {
                filter = value;
                RaisePropertyChanged("FilteredContacts");
            }
        }
        public ObservableCollection<Contact> FilteredContacts
        {
            get
            {
                if (string.IsNullOrEmpty(ContactsFilter))
                    return ContactsList;
                var col = ContactsList.Where(con => con.Name.ToLower().Contains(ContactsFilter.ToLower())
                || con.Surname.ToLower().Contains(ContactsFilter.ToLower())).ToList();
                return new ObservableCollection<Contact>(col);
            }
        }

        private RelayCommand addContactCommand;
        public RelayCommand AddContactCommand => addContactCommand ?? (addContactCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<ExternalContact>(new ExternalContact(), "AddContact"));
                    navigationService.Navigate<CreateNewContactPageView>();
                }
            ));

        private RelayCommand<Contact> deleteContactCommand;
        public RelayCommand<Contact> DeleteContactCommand => deleteContactCommand ?? (deleteContactCommand = new RelayCommand<Contact>(
                param =>
                {

                }
            ));
        private RelayCommand<Contact> editContactCommand;
        public RelayCommand<Contact> EditContactCommand => editContactCommand ?? (editContactCommand = new RelayCommand<Contact>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<ExternalContact>(param as ExternalContact, "ChangeContact"));
                    navigationService.Navigate<CreateNewContactPageView>();
                }
            ));
        private RelayCommand<Contact> showContactCommand;
        public RelayCommand<Contact> ShowContactCommand => showContactCommand ?? (showContactCommand = new RelayCommand<Contact>(
                param =>
                {
                    var str = new StringBuilder();
                   // str.Append(param.Name+ " " + param.Surname+"\n");
                    if (param is Employee)
                    {
                        var val = param as Employee;
                        str.Append(val.Company.CompanyName + " ("+val.Department.DepartmentName+" "+val.Position.PositionName+")\n");
                    }
                    else if (param is ExternalContact)
                    {
                        var val = param as ExternalContact;
                        str.Append(val.Organization.OrganizationName + "\n");
                    }
                    foreach (var item in param.ContactInfos)
                    {
                        str.Append(item.ContactInfoType.InfoType+": "+item.Value+"\n");
                    }

                    messageService.SelectableInfo(str.ToString(), param.Name + " " + param.Surname,param.Photo);
                }
            ));


        #region NavigationCommands
        //Upper Menu
        private RelayCommand gMain;
        public RelayCommand GMain => gMain ?? (gMain = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));

        private RelayCommand gSettings;
        public RelayCommand GSettings => gSettings ?? (gSettings = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<SettingsPageView>();
                }
            ));

        private RelayCommand gExit;
        public RelayCommand GExit => gExit ?? (gExit = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<LogInPageView>();
                }
            ));


        //Aside

        private RelayCommand gSchedule;
        public RelayCommand GSchedule => gSchedule ?? (gSchedule = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<SchedulePageView>();
                }
            ));

        private RelayCommand gDocuments;
        public RelayCommand GDocuments => gDocuments ?? (gDocuments = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand gNews;
        public RelayCommand GNews => gNews ?? (gNews = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<NewsPageView>();
                }
            ));

        private RelayCommand gCalendar;
        public RelayCommand GCalendar => gCalendar ?? (gCalendar = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<CalendarPageView>();
                }
            ));

        private RelayCommand gMail;
        public RelayCommand GMail => gMail ?? (gMail = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<GMailPageView>();
                }
            ));

        private RelayCommand gContacts;
        public RelayCommand GContacts => gContacts ?? (gContacts = new RelayCommand(
                () =>
                {

                    navigationService.Navigate<ContactsPageView>();
                }
            ));
        #endregion
    }
}
