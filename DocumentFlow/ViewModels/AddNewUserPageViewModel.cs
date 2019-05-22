using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class AddNewUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private bool isActive;
        public bool IsActive { get => isActive; set => Set(ref isActive, value); }

        private bool headOfDep;
        public bool HeadOfDep { get => headOfDep; set => Set(ref headOfDep, value); }

        private bool canEditContacts;
        public bool CanEditContacts { get => canEditContacts; set => Set(ref canEditContacts, value); }

        private string googleAccount;
        public string GoogleAccount { get => googleAccount; set => Set(ref googleAccount, value); }
        private string name;
        public string Name { get => name; set => Set(ref name, value); }
        private string surname;
        public string Surname { get => surname; set => Set(ref surname, value); }

        private Company company;
        public Company Company { get => company; set => Set(ref company, value); }

        private Department department;
        public Department Department { get => department; set => Set(ref department, value); }

        private Position position;
        public Position Position { get => position; set => Set(ref position, value); }

        private ObservableCollection<Company> companyCollection;
        public ObservableCollection<Company> CompanyCollection { get => companyCollection; set => Set(ref companyCollection, value); }

        private ObservableCollection<Department> departmentCollection;
        public ObservableCollection<Department> DepartmentCollection { get => departmentCollection; set => Set(ref departmentCollection, value); }

        private ObservableCollection<Position> positionCollection;
        public ObservableCollection<Position> PositionCollection { get => positionCollection; set => Set(ref positionCollection, value); }

        private string passCheckError;
        public string PassCheckError { get => passCheckError; set => Set(ref passCheckError, value); }

        private bool passwordConfirmation;
        public bool PasswordConfirmation { get => passwordConfirmation; set => Set(ref passwordConfirmation, value); }

        private string buttonOkContent;
        public string ButtonOkContent { get => buttonOkContent; set => Set(ref buttonOkContent, value); }

        private bool gAccountIsReadOnly;
        public bool GAccountIsReadOnly { get => gAccountIsReadOnly; set => Set(ref gAccountIsReadOnly, value); }

        private User CurrentUser { get; set; }

        public AddNewUserPageViewModel(INavigationService navigationService, 
                                          IMessageService messageService, 
                                             AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitIt);
        }

        private void OnHitIt(NotificationMessage<User> usr)
        {
            if (usr.Notification == "OpenToAdd")
            {
                GAccountIsReadOnly = false;
                ButtonOkContent = "Create";
                Department = null;
                Position = null;
                Company = null;
            }
            else if (usr.Notification == "OpenToEdit")
            {
                GAccountIsReadOnly = true;
                ButtonOkContent = "Save";
                CurrentUser = usr.Content;
                IsActive = CurrentUser.IsActive;
                GoogleAccount = CurrentUser.GoogleAccount;
                var CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                HeadOfDep = CurrentEmployee.HeadOfDep;
                CanEditContacts = CurrentEmployee.CanEditContacts;
                Name = CurrentEmployee.Name;
                Surname = CurrentEmployee.Surname;
                Department = CurrentEmployee.Department;
                Position = CurrentEmployee.Position;
                Company = CurrentEmployee.Company;
            }
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(UserControlOpened));
        private void UserControlOpened()
        {
            CompanyCollection = new ObservableCollection<Company>(db.Companies);
            DepartmentCollection = new ObservableCollection<Department>(db.Departments);
            PositionCollection = new ObservableCollection<Position>(db.Positions);
            PasswordConfirmation = false;
            PassCheckError = "";
            if (!GAccountIsReadOnly)
            {
                IsActive = true;
                GoogleAccount = Name = Surname = "";
            }
        }

        private RelayCommand lostFocusCommand_GoogleAccount;
        public RelayCommand LostFocusCommand_GoogleAccount => lostFocusCommand_GoogleAccount ?? (lostFocusCommand_GoogleAccount = new RelayCommand(
                () =>
                {
                    if (db.Users.Where(u => u.GoogleAccount.Equals(GoogleAccount)).Any())
                    {
                        // This Google account is already presented in DB
                        messageService.ShowError("Wrong Google account!");
                        GoogleAccount = "";
                    }
                }
                 ));

        private RelayCommand<object> lostFocusCommand_pBox;
        public RelayCommand<object> LostFocusCommand_pBox
        {
            get => lostFocusCommand_pBox ?? (lostFocusCommand_pBox = new RelayCommand<object>(
                param =>
                {
                    var passwordContainer = param as IPasswordSupplier;
                    if (passwordContainer != null)
                    {
                        var chk = passwordContainer.ConfirmPassword();
                        if (chk)
                        {
                            PassCheckError = "";
                            PasswordConfirmation = true;
                        }
                        else
                        {
                            PassCheckError = "Passwords don't match";
                            PasswordConfirmation = false;
                        }
                    }
                }
            ));
        }

        private RelayCommand goBackCommand;
        public RelayCommand GoBackCommand => goBackCommand ?? (goBackCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AdminPanelPageView>();
                }
            ));

        private RelayCommand<object> createNewUserCommand;
        public RelayCommand<object> CreateNewUserCommand
        {
            get => createNewUserCommand ?? (createNewUserCommand = new RelayCommand<object>(
                async param =>
                {
                    if (ButtonOkContent == "Create")
                    {
                        var chkOk = true;
                        var errorStr = new StringBuilder("Following fiends cannot be empty:\n");
                        if (string.IsNullOrEmpty(GoogleAccount))
                        {
                            errorStr.Append("Google accont\n");
                            chkOk = false;
                        }
                        if (string.IsNullOrEmpty(Name))
                        {
                            errorStr.Append("Name\n");
                            chkOk = false;
                        }
                        if (string.IsNullOrEmpty(Surname))
                        {
                            errorStr.Append("Surname\n");
                            chkOk = false;
                        }
                        if (Company == null)
                        {
                            errorStr.Append("Company\n");
                            chkOk = false;
                        }
                        if (Department == null)
                        {
                            errorStr.Append("Department\n");
                            chkOk = false;
                        }
                        if (Position == null)
                        {
                            errorStr.Append("Position\n");
                            chkOk = false;
                        }
                        if (!PasswordConfirmation)
                        {
                            errorStr.Append("(!) Passwords doesn't match!\n");
                            chkOk = false;
                        }

                        if (chkOk)
                        {
                            var passwordContainer = param as IPasswordSupplier;
                            if (passwordContainer != null)
                            {
                                var sPass = passwordContainer.GetPassword;
                                RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                                byte[] salt = new byte[32];
                                csprng.GetBytes(salt);
                                var saltValue = Convert.ToBase64String(salt);

                                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + new NetworkCredential(string.Empty, sPass).Password);
                                SHA256Managed hashstring = new SHA256Managed();
                                byte[] hash = hashstring.ComputeHash(saltedPassword);
                                saltValue = Convert.ToBase64String(salt);
                                var hashValue = Convert.ToBase64String(hash);

                                // var login = GoogleAccount.Substring(GoogleAccount.IndexOf('@') + 1); 
                                var login = GoogleAccount.Substring(0, GoogleAccount.IndexOf('@'));

                                var NewUser = new User
                                {
                                    IsActive = IsActive,
                                    GoogleAccount = GoogleAccount,
                                    Login = login,
                                    SaltValue = saltValue,
                                    HashValue = hashValue
                                };

                                db.Users.Add(NewUser);

                                var emp = new Employee
                                {
                                    CanEditContacts = CanEditContacts,
                                    Company = Company,
                                    Department = Department,
                                    HeadOfDep = HeadOfDep,
                                    Name = Name,
                                    Surname = Surname,
                                    Photo = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\Images\\user.png",
                                    Position = Position,
                                    User = NewUser,
                                    Language = db.Languages.Where(l => l.Id == 1).Single(),
                                    ColorScheme = db.ColorSchemes.Where(l => l.Id == 1).Single()
                                };
                                db.Contacts.Add(emp);
                                await db.SaveChangesAsync();
                            }

                            navigationService.Navigate<AdminPanelPageView>();
                        }
                    }
                    else // Save changes
                    {
                        CurrentUser.IsActive = IsActive;

                        if (!PasswordConfirmation)
                        {
                            var passwordContainer = param as IPasswordSupplier;
                            if (passwordContainer != null)
                            {
                                if (!passwordContainer.IsEmpty())
                                {
                                    var sPass = passwordContainer.GetPassword;
                                    RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                                    byte[] salt = new byte[32];
                                    csprng.GetBytes(salt);
                                    var saltValue = Convert.ToBase64String(salt);

                                    byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + new NetworkCredential(string.Empty, sPass).Password);
                                    SHA256Managed hashstring = new SHA256Managed();
                                    byte[] hash = hashstring.ComputeHash(saltedPassword);
                                    saltValue = Convert.ToBase64String(salt);
                                    var hashValue = Convert.ToBase64String(hash);
                                    CurrentUser.SaltValue = saltValue;
                                    CurrentUser.HashValue = hashValue;
                                }
                            }
                        }
                        var CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
                        CurrentEmployee.CanEditContacts = CanEditContacts;
                        CurrentEmployee.Company = Company;
                        CurrentEmployee.Department = Department;
                        CurrentEmployee.HeadOfDep = HeadOfDep;
                        CurrentEmployee.Name = Name;
                        CurrentEmployee.Position = Position;
                        CurrentEmployee.Surname = Surname;

                        await db.SaveChangesAsync();
                        navigationService.Navigate<AdminPanelPageView>();
                    }


                }
            ));
        }
    }
}
