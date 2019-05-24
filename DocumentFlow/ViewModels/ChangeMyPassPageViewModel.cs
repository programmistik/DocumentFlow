using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class ChangeMyPassPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User user;
        public User CurrentUser { get => user; set => Set(ref user, value); }

        private string passwordConfirmation;
        public string PasswordConfirmation { get => passwordConfirmation; set => Set(ref passwordConfirmation, value); }

        public ChangeMyPassPageViewModel(INavigationService navigationService,
                                             IMessageService messageService,
                                                AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, usr =>
            {
                CurrentUser = usr.Content;
            });
        }

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
                            PasswordConfirmation = "";
                        else
                            PasswordConfirmation = "Passwords don't match";
                    }
                }
            ));
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    if (CurrentUser.Login == "admin")
                        navigationService.Navigate<AdminPanelPageView>();
                    else
                    {
                        Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                        navigationService.Navigate<SettingsPageView>();
                    }
                }
            ));
        }

        private RelayCommand<object> okCommand;
        public RelayCommand<object> OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand<object>(
                async param =>
                {
                    var passwordContainer = param as IPasswordSupplier;
                    if (passwordContainer != null)
                    {
                        var curr = passwordContainer.GetCurrentPassword;

                        string saltValueFromDB = CurrentUser.SaltValue;
                        string hashValueFromDB = CurrentUser.HashValue;

                        byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValueFromDB + new NetworkCredential(string.Empty, curr).Password);
                        SHA256Managed hashstring = new SHA256Managed();
                        byte[] hash = hashstring.ComputeHash(saltedPassword);
                        string hashToCompare = Convert.ToBase64String(hash);
                        if (hashValueFromDB == null || hashValueFromDB.Equals(hashToCompare))
                        {
                            
                            if (!passwordContainer.IsEmpty())
                            {
                                if (passwordContainer.ConfirmPassword())
                                {
                                    var newPass = passwordContainer.GetPassword;
                                    RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                                    byte[] salt = new byte[32];
                                    csprng.GetBytes(salt);
                                    var saltValue = Convert.ToBase64String(salt);

                                    saltedPassword = Encoding.UTF8.GetBytes(saltValue + new NetworkCredential(string.Empty, newPass).Password);
                                    hashstring = new SHA256Managed();
                                    hash = hashstring.ComputeHash(saltedPassword);
                                    saltValue = Convert.ToBase64String(salt);
                                    var hashValue = Convert.ToBase64String(hash);

                                    CurrentUser.SaltValue = saltValue;
                                    CurrentUser.HashValue = hashValue;

                                    await db.SaveChangesAsync();
                                    messageService.ShowInfo("Password has been changed!");

                                    if (CurrentUser.Login == "admin")
                                        navigationService.Navigate<AdminPanelPageView>();
                                    else
                                    {
                                        Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                                        navigationService.Navigate<SettingsPageView>();
                                    }
                                }
                                else
                                    messageService.ShowError("New passwords confirmation failed!");
                            }
                            else
                                messageService.ShowError("New password can't be empty!");

                        }
                        else
                            messageService.ShowError("Current password is wrong!");
                    }


                }
            ));
        }
    }
}