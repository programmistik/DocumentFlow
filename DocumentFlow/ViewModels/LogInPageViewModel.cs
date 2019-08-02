using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DocumentFlow.ViewModels
{
    class LogInPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private BitmapImage image;
        public BitmapImage Image { get => image; set => Set(ref image, value); }

        private string loginCheck;
        public string LoginCheck { get => loginCheck; set => Set(ref loginCheck, value); }

        private string checkColor;
        public string CheckColor { get => checkColor; set => Set(ref checkColor, value); }

        private string loginUserName;
        public string LoginUserName { get => loginUserName; set => Set(ref loginUserName, value); }

        private SecureString pass;
        public SecureString Pass { get => pass; set => Set(ref pass, value); }


        public LogInPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
        }

        
        private RelayCommand<object> logInCommand;
        public RelayCommand<object> LogInCommand
        {
            get => logInCommand ?? (logInCommand = new RelayCommand<object>(
                param =>
                {
                    if (LoginUserName != null && LoginUserName.ToLower().Contains("admin"))
                    {
                        var admin = db.Users.Where(usr => usr.Login == "admin").Single();
                        if (admin.HashValue == null)
                            navigationService.Navigate<AdminPanelPageView>();
                        else
                        {
                            var passwordContainer = param as IPasswordSupplier;
                            if (passwordContainer != null)
                            {
                                var sPass = passwordContainer.GetPassword;

                                string saltValueFromDB = admin.SaltValue;
                                string hashValueFromDB = admin.HashValue;

                                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValueFromDB + new NetworkCredential(string.Empty, sPass).Password);
                                SHA256Managed hashstring = new SHA256Managed();
                                byte[] hash = hashstring.ComputeHash(saltedPassword);
                                string hashToCompare = Convert.ToBase64String(hash);
                                if (hashValueFromDB.Equals(hashToCompare))
                                    navigationService.Navigate<AdminPanelPageView>();
                                else
                                    messageService.ShowError("Login credentials incorrect. User not validated.");
                            }
                        }
                    }
                    else
                    {
                        var qwr = db.Users.Where(u => u.Login == LoginUserName);


                        if (qwr.Any() == false)
                        {

                            qwr = db.Users.Where(u => u.GoogleAccount == LoginUserName);
                            if (qwr.Any() == false)
                            {
                                messageService.ShowError("Username not found.");
                                return;
                            }

                        }
                        var passwordContainer = param as IPasswordSupplier;
                        if (passwordContainer != null)
                        {
                            var sPass = passwordContainer.GetPassword;

                            string saltValueFromDB = qwr.Single().SaltValue;
                            string hashValueFromDB = qwr.Single().HashValue;

                            byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValueFromDB + new NetworkCredential(string.Empty, sPass).Password);
                            SHA256Managed hashstring = new SHA256Managed();
                            byte[] hash = hashstring.ComputeHash(saltedPassword);
                            string hashToCompare = Convert.ToBase64String(hash);
                            if (hashValueFromDB.Equals(hashToCompare))
                            {
                                var Usr = qwr.Single();
                                if (Usr.IsActive){
                                    Messenger.Default.Send(new NotificationMessage<User>(Usr, "SendCurrentUser"));
                                    //navigationService.Navigate<MainPageView>();
                                    var emp = db.Employees.Where(e => e.UserId == Usr.Id).Single();
                                    var lanCode = emp.Language.LangCultureCode;
                                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lanCode);
                                    navigationService.Navigate<MainDesktopPageView>();
                                 }
                                else
                                {
                                    messageService.ShowError("Your account is deactiveted. Contact administrator");
                                }
                            }
                            else
                                messageService.ShowError("Login credentials incorrect. User not validated.");
                        }
                        //navigationService.Navigate<MainDesktopPageView>();
                    }
                }

            ));
        }



        private RelayCommand forgotPasswordCommand;
        public RelayCommand ForgotPasswordCommand
        {
            get => forgotPasswordCommand ?? (forgotPasswordCommand = new RelayCommand(
                () =>
                {
                    messageService.ShowInfo("Contact administrator");

                }
            ));
        }

        private RelayCommand<string> lostFocusCommand;
        public RelayCommand<string> LostFocusCommand
        {
            get => lostFocusCommand ?? (lostFocusCommand = new RelayCommand<string>(
                param =>
                {
                }
            ));
        }
    }
}