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

        private string buttonOkContent;
        public string ButtonOkContent { get => buttonOkContent; set => Set(ref buttonOkContent, value); }

        public CreateNewContactPageViewModel(INavigationService navigationService,
                                           IMessageService messageService,
                                              AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            OrgCollection = new ObservableCollection<Organization>(db.Organizations);

            Messenger.Default.Register<NotificationMessage<Contact>>(this, OnHitIt);
        }

        private void OnHitIt(NotificationMessage<Contact> cont)
        {
            if (cont.Notification == "ChangeContact")
            {
                ButtonOkContent = "Save";
            }
            //else if (cont.Notification == "CreateContact")
            //{
            //    ButtonOkContent = "Create";
            //}
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    
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
                    var lst = db.ContactInfoTypes.ToList();

                    var win = new AddContactInfoWindow(lst);

                    win.ShowDialog();
                    if (!string.IsNullOrEmpty(win.InputValue.Text))
                    {
                        //var newInfo = new ContactInformation
                        //{
                        //    Contact = CurrentEmployee,
                        //    ContactInfoType = win.InfoCollection.SelectedItem as ContactInfoType,
                        //    Value = win.InputValue.Text
                        //};
                        //InfoList.Add(newInfo);
                        ////db.Employees.Where(e => e == CurrentEmployee).Single().ContactInfos.Add(newInfo);
                        //CurrentEmployee.ContactInfos.Add(newInfo);
                    }

                }
            ));

    }
}
