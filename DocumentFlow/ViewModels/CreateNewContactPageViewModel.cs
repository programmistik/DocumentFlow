using DocumentFlow.ModalWindows;
using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class CreateNewContactPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private string name;
        public string Name { get => name; set => Set(ref name, value); }

        private string surname;
        public string Surname { get => surname; set => Set(ref surname, value); }

        private Organization organization;
        public Organization Organization { get => organization; set => Set(ref organization, value); }

        private ObservableCollection<Organization> orgCollection;
        public ObservableCollection<Organization> OrgCollection { get => orgCollection; set => Set(ref orgCollection, value); }

        private ObservableCollection<ContactInformation> infoList;
        public ObservableCollection<ContactInformation> InfoList { get => infoList; set => Set(ref infoList, value); }

        private string buttonOkContent;
        public string ButtonOkContent { get => buttonOkContent; set => Set(ref buttonOkContent, value); }

        private ExternalContact CurrentContact { get; set; }

        public CreateNewContactPageViewModel(INavigationService navigationService,
                                           IMessageService messageService,
                                              AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;


            Messenger.Default.Register<NotificationMessage<ExternalContact>>(this, OnHitIt);
        }

        private void OnHitIt(NotificationMessage<ExternalContact> cont)
        {
            OrgCollection = new ObservableCollection<Organization>(db.Organizations);
            
            if (cont.Notification == "ChangeContact")
            {
                ButtonOkContent = Properties.Resources.Save; //"Save";
                CurrentContact = cont.Content;
                Name = CurrentContact.Name;
                Surname = CurrentContact.Surname;
                Organization = CurrentContact.Organization;
                InfoList = new ObservableCollection<ContactInformation>(CurrentContact.ContactInfos);
                //InfoList = new ObservableCollection<ContactInformation>(db.ContactInformations.Where(x => x.ContactId == CurrentContact.Id));
            }
            else if (cont.Notification == "AddContact")
            {
                ButtonOkContent = Properties.Resources.Create; //"Create";
                CurrentContact = cont.Content;
                InfoList = new ObservableCollection<ContactInformation>();
            }
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    // clear db context

                    db.ClearContext();

                    navigationService.Navigate<ContactsPageView>();
                   
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                async () =>
                {
                    ///
                    CurrentContact.Name = Name;
                    CurrentContact.Surname = Surname;
                    CurrentContact.Organization = Organization;

                    if (ButtonOkContent == "Create")
                        db.Contacts.Add(CurrentContact);

                    await db.SaveChangesAsync();
                    navigationService.Navigate<ContactsPageView>();

                }
            ));
        }

        private RelayCommand addNewOrgCommans;
        public RelayCommand AddNewOrgCommans
        {
            get => addNewOrgCommans ?? (addNewOrgCommans = new RelayCommand(
                async() =>
                {
                    var OrgWin = new AddOrganizationWindow();
                    OrgWin.ShowDialog();
                    if (OrgWin.CreateNew)
                    {
                        if (!string.IsNullOrEmpty(OrgWin.InputValue.Text))
                        {
                            if (db.Organizations.Where(org => org.OrganizationName.ToLower() == OrgWin.InputValue.Text.ToLower()).Any())
                            {
                                messageService.ShowError("Organization with this name is already exists!");
                                Organization = db.Organizations.Where(org => org.OrganizationName.ToLower() == OrgWin.InputValue.Text.ToLower()).Single();
                            }
                            else
                            {
                                var newOrg = new Organization { OrganizationName = OrgWin.InputValue.Text };
                                OrgCollection.Add(newOrg);
                                Organization = newOrg;
                                db.Organizations.Add(newOrg);
                                await db.SaveChangesAsync();
                                messageService.ShowInfo($"Organization {Organization.OrganizationName} has been created!");
                            }
                        }
                        else
                        {
                            messageService.ShowError("You can't create organization with empty name!");
                        }
                    }
                }
            ));
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand => addCommand ?? (addCommand = new RelayCommand(
                () =>
                {
                    var lst = db.ContactInfoTypes;

                    var win = new AddContactInfoWindow(lst.ToList(), lst.FirstOrDefault(), "");

                    win.ShowDialog();
                    if (win.DataContext is AddContactInfoViewModel)
                    {
                        var dc = win.DataContext as AddContactInfoViewModel;
                        if (!string.IsNullOrEmpty(dc.InputValue))

                        {
                            var newInfo = new ContactInformation
                            {
                                Contact = CurrentContact,
                                ContactInfoType = dc.SelectedInfo,
                                Value = dc.InputValue
                            };
                            InfoList.Add(newInfo);
                            //db.Employees.Where(e => e == CurrentEmployee).Single().ContactInfos.Add(newInfo);
                            CurrentContact.ContactInfos.Add(newInfo);
                        }

                    }
                }
            ));

        private RelayCommand<ContactInformation> deleteInfoCommand;
        public RelayCommand<ContactInformation> DeleteInfoCommand => deleteInfoCommand ?? (deleteInfoCommand = new RelayCommand<ContactInformation>(
                param =>
                {
                    var answer = messageService.ShowYesNo(Properties.Resources.AreYouSure + "?");
                    if (answer)
                    {
                        InfoList.Remove(param);
                    }
                }
            ));

        private RelayCommand<ContactInformation> editInfoCommand;
        public RelayCommand<ContactInformation> EditInfoCommand => editInfoCommand ?? (editInfoCommand = new RelayCommand<ContactInformation>(
                param =>
                {
                    
                    var lst = db.ContactInfoTypes;

                    var win = new AddContactInfoWindow(lst.ToList(), param.ContactInfoType, param.Value);

                    win.ShowDialog();
                    if (win.DataContext is AddContactInfoViewModel)
                    {
                        var dc = win.DataContext as AddContactInfoViewModel;
                        if (!string.IsNullOrEmpty(dc.InputValue))

                        {
                            param.ContactInfoType = dc.SelectedInfo;
                            param.Value = dc.InputValue;
                            InfoList = new ObservableCollection<ContactInformation>(CurrentContact.ContactInfos);
                        }

                    }
                }
            ));

    }
}
