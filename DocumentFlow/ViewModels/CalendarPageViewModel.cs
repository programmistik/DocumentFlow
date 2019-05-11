using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DocumentFlow.Views;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace DocumentFlow.ViewModels
{
    public class CalendarPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;

        private DateTime? selectedDate;
        public DateTime? SelectedDate { get => selectedDate; set => Set(ref selectedDate, value); }
        private DateTime currentDate;
        public DateTime CurrentDate { get => currentDate; set => Set(ref currentDate, value); }

        private ObservableCollection<Event> eventList;
        public ObservableCollection<Event> EventList { get => eventList; set => Set(ref eventList, value); }

        private CalendarService GoogleCalendarService;
        public CalendarPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GoogleCalendarService = googleService.GetQuickstartService();
            CurrentDate = DateTime.Today;
            SelectedDate = (DateTime?)CurrentDate;
            var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
            EventList = new ObservableCollection<Event>(events.Items);
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            GoogleCalendarService = googleService.GetQuickstartService();
            CurrentDate = DateTime.Today;
            SelectedDate = (DateTime?)CurrentDate;
            var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
            EventList = new ObservableCollection<Event>(events.Items);
        }

        private RelayCommand<SelectionChangedEventArgs> selectedDatesChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> SelectedDatesChangedCommand => selectedDatesChangedCommand ?? (selectedDatesChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
                    EventList.Clear();
                    SelectedDate = ((System.Windows.Controls.Calendar)param.Source).SelectedDate;
                    if (SelectedDate != null)
                    {
                        var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                        EventList = new ObservableCollection<Event>(events.Items);
                    }
                }
            ));
        private RelayCommand addEvent;
        public RelayCommand AddEvent => addEvent ?? (addEvent = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<Event>(new Event(), "EventToAdd"));
                    Messenger.Default.Send(new NotificationMessage<CalendarService>(GoogleCalendarService, "CurrentGoogleService"));
                    navigationService.Navigate<AddEditEventPageView>();

                }
                 ));

        private RelayCommand<Event> deleteEventCommand;
        public RelayCommand<Event> DeleteEventCommand => deleteEventCommand ?? (deleteEventCommand = new RelayCommand<Event>(
                param =>
                {
                    if (messageService.ShowYesNo("Are you sure?"))
                    {
                        googleService.deleteEvent(GoogleCalendarService, param);

                        var events = googleService.GetEventsByDate((DateTime)SelectedDate, GoogleCalendarService);
                        EventList = new ObservableCollection<Event>(events.Items);
                    }
                }
                 ));

        private RelayCommand<Event> editEventCommand;
        public RelayCommand<Event> EditEventCommand => editEventCommand ?? (editEventCommand = new RelayCommand<Event>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<Event>(param, "EventToEdit"));
                    Messenger.Default.Send(new NotificationMessage<CalendarService>(GoogleCalendarService, "CurrentGoogleService"));
                    navigationService.Navigate<AddEditEventPageView>();
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