﻿using DocumentFlow.Models;
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
    public class DocumentsPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User CurrentUser { get; set; }

        private string fio;
        public string Fio { get => fio; set => Set(ref fio, value); }

        private string avatara;
        public string Avatara { get => avatara; set => Set(ref avatara, value); }

        private ObservableCollection<Document> docsCollection;
        public ObservableCollection<Document> DocsCollection { get => docsCollection; set => Set(ref docsCollection, value); }


        public DocumentsPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);

        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(
        () =>
        {
            var emp = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();

            if (emp.HeadOfDep)
            {
                DocsCollection = new ObservableCollection<Document>(db.Documents.Where(d => d.CreatedBy.Id == CurrentUser.Id ||
                d.myProcesses.Any(p => p.StartUser.Id == CurrentUser.Id) || d.myProcesses.Any(p => p.TaskUser.Id == CurrentUser.Id) ||
                d.myProcesses.Any(p => p.Department.Id == emp.Department.Id) ));
            }
            else
            {
                var docs = db.Documents.Where(d => d.CreatedBy.Id == CurrentUser.Id ||
                d.myProcesses.Any(p => p.StartUser.Id == CurrentUser.Id) || d.myProcesses.Any(p => p.TaskUser.Id == CurrentUser.Id)).ToList();

                DocsCollection = new ObservableCollection<Document>(docs);
            }

        }));

        private void OnHitUser(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUser = usr.Content;
                var emp = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                Fio = emp.Name + " " + emp.Surname;

                if (string.IsNullOrEmpty(emp.Photo))
                    Avatara = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Images\\user.png";
                else
                    Avatara = emp.Photo;
            }
        }

        private RelayCommand addNewDocument;
        public RelayCommand AddNewDocument => addNewDocument ?? (addNewDocument = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    Messenger.Default.Send(new NotificationMessage<Document>(new Document(), "NewDocument"));
                    navigationService.Navigate<DocPageView>();
                }
            ));

        private RelayCommand<Document> doubleClickCommand;
        public RelayCommand<Document> DoubleClickCommand => doubleClickCommand ?? (doubleClickCommand = new RelayCommand<Document>(
        param =>
        {
            Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
            Messenger.Default.Send(new NotificationMessage<Document>(param, "CurrentDocument"));
            navigationService.Navigate<DocPageView>();
        }));

        #region NavigationCommands
        //Upper Menu
        private RelayCommand gMain;
        public RelayCommand GMain => gMain ?? (gMain = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<MainDesktopPageView>();
                }
            ));

        private RelayCommand gSettings;
        public RelayCommand GSettings => gSettings ?? (gSettings = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
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
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<SchedulePageView>();
                }
            ));

        private RelayCommand gNews;
        public RelayCommand GNews => gNews ?? (gNews = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<NewsPageView>();
                }
            ));

        private RelayCommand gCalendar;
        public RelayCommand GCalendar => gCalendar ?? (gCalendar = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<CalendarPageView>();
                }
            ));

        private RelayCommand gMail;
        public RelayCommand GMail => gMail ?? (gMail = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<GMailPageView>();
                }
            ));

        private RelayCommand gContacts;
        public RelayCommand GContacts => gContacts ?? (gContacts = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "Contacts"));
                    navigationService.Navigate<ContactsPageView>();
                }
            ));


        #endregion
    }
}
