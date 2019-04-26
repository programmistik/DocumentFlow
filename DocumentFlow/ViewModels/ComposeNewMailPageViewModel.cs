using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Services.WebBrowserServices;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlow.ViewModels
{
    public class ComposeNewMailPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;
        private GmailService GMailService;

        private string textTo;
        public string TextTo { get => textTo; set => Set(ref textTo, value); }
        private string textSubject;
        public string TextSubject { get => textSubject; set => Set(ref textSubject, value); }


        public ComposeNewMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;

            TextTo = "programmistik@yahoo.com";
            TextSubject = "Test";

            Messenger.Default.Register<NotificationMessage<GmailService>>(this, goo =>
            {
                GMailService = goo.Content;
            });

        }

        private RelayCommand closeCommand;
        public RelayCommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<GMailPageView>();
                }
                 ));

        private RelayCommand sendMailCommand;
        public RelayCommand SendMailCommand => sendMailCommand ?? (sendMailCommand = new RelayCommand(
                () =>
                {
                    dynamic doc = Gui.webBrowser.doc;
                    var htmlText = doc.documentElement.InnerHtml;


                    var msg = new Message();
                    msg.Payload = new MessagePart();

                    msg.Payload.Headers = new List<MessagePartHeader>();
                    
                    msg.Payload.Headers.Add(new MessagePartHeader
                    {
                        Name = "Date",
                        Value = DateTime.Now.ToString()
                    });
                    msg.Payload.Headers.Add(new MessagePartHeader
                    {
                        Name = "From",
                        Value = "me"
                    });
                    msg.Payload.Headers.Add(new MessagePartHeader
                    {
                        Name = "To",
                        Value = TextTo
                    });
                    msg.Payload.Headers.Add(new MessagePartHeader
                    {
                        Name = "Subject",
                        Value = TextSubject
                    });
                    var msgPart = new MessagePart();
                    msgPart.MimeType = "text/html";
                    msgPart.Body = new MessagePartBody();

                    msgPart.Body.Data = googleService.Base64UrlEncode(Encoding.ASCII.GetBytes(htmlText));

                    msg.Payload.Parts = new List<MessagePart>();
                    msg.Payload.Parts.Add(msgPart);

                    msg.Raw = googleService.CreateRawForNewMessage(TextTo,TextSubject,htmlText);

                    // send message
                    try
                    {
                        GMailService.Users.Messages.Send(msg, "me").Execute();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An error occurred: " + e.Message);
                    }

                    navigationService.Navigate<GMailPageView>();
                }
                 ));

        #region RibbonButtonsCommands

        private RelayCommand boldCommand;
        public RelayCommand BoldCommand => boldCommand ?? (boldCommand = new RelayCommand(
                () =>
                {
                    Format.bold();
                }
                 ));

        private RelayCommand italicCommand;
        public RelayCommand ItalicCommand => italicCommand ?? (italicCommand = new RelayCommand(
                () =>
                {
                    Format.Italic();
                }
                 ));

        private RelayCommand underLineCommand;
        public RelayCommand UnderLineCommand => underLineCommand ?? (underLineCommand = new RelayCommand(
                () =>
                {
                    Format.Underline();
                }
                 ));

        private RelayCommand fontColorCommand;
        public RelayCommand FontColorCommand => fontColorCommand ?? (fontColorCommand = new RelayCommand(
                () =>
                {
                    Gui.SettingsFontColor();
                }
                 ));

        private RelayCommand addLinkCommand;
        public RelayCommand AddLinkCommand => addLinkCommand ?? (addLinkCommand = new RelayCommand(
                () =>
                {
                    Gui.SettingsAddLink();
                }
                 ));

        private RelayCommand addImageCommand;
        public RelayCommand AddImageCommand => addImageCommand ?? (addImageCommand = new RelayCommand(
                () =>
                {
                    //Gui.SettingsAddImage();
                }
                 ));

        private RelayCommand leftAlignCommand;
        public RelayCommand LeftAlignCommand => leftAlignCommand ?? (leftAlignCommand = new RelayCommand(
                () =>
                {
                    Format.JustifyLeft();
                }
                 ));

        private RelayCommand center2Command;
        public RelayCommand Center2Command => center2Command ?? (center2Command = new RelayCommand(
                () =>
                {
                    Format.JustifyCenter();
                }
                 ));

        private RelayCommand rightAlignCommand;
        public RelayCommand RightAlignCommand => rightAlignCommand ?? (rightAlignCommand = new RelayCommand(
                () =>
                {
                    Format.JustifyRight();
                }
                 ));

        private RelayCommand centerCommand;
        public RelayCommand CenterCommand => centerCommand ?? (centerCommand = new RelayCommand(
                () =>
                {
                    Format.JustifyFull();
                }
                 ));

        private RelayCommand numberedCommand;
        public RelayCommand NumberedCommand => numberedCommand ?? (numberedCommand = new RelayCommand(
                () =>
                {
                    Format.InsertOrderedList();
                }
                 ));


        private RelayCommand bulletsCommand;
        public RelayCommand BulletsCommand => bulletsCommand ?? (bulletsCommand = new RelayCommand(
                () =>
                {
                    Format.InsertUnorderedList();
                }
                 ));

        private RelayCommand outIdentCommand;
        public RelayCommand OutIdentCommand => outIdentCommand ?? (outIdentCommand = new RelayCommand(
                () =>
                {
                    Format.Outdent();
                }
                 ));

        private RelayCommand identCommand;
        public RelayCommand IdentCommand => identCommand ?? (identCommand = new RelayCommand(
                () =>
                {
                    Format.Indent();
                }
                 ));

        #endregion

        private RelayCommand editWeb1Command;
        public RelayCommand EditWeb1Command => editWeb1Command ?? (editWeb1Command = new RelayCommand(
                () =>
                {
                    Gui.EditWeb();
                }
                 ));

        private RelayCommand viewHTMLCommand;
        public RelayCommand ViewHTMLCommand => viewHTMLCommand ?? (viewHTMLCommand = new RelayCommand(
                () =>
                {
                    Gui.ViewHTML();
                }
                 ));

        //private RelayCommand<SelectionChangedEventArgs> fontsChangedCommand;
        //public RelayCommand<SelectionChangedEventArgs> FontsChangedCommand => fontsChangedCommand ?? (fontsChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
        //        async param =>
        //        {
        //            //Gui.RibbonComboboxFonts((ComboBox)param.Source);
        //            var ok = await Gui.RibbonComboboxFontsAsync((ComboBox)param.Source);
        //        }
        //    ));
        private RelayCommand<SelectionChangedEventArgs> fontsChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> FontsChangedCommand => fontsChangedCommand ?? (fontsChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
                    Gui.RibbonComboboxFonts((ComboBox)param.Source);
                }
            ));

        private RelayCommand<SelectionChangedEventArgs> fontHeightChangedCommand;
        public RelayCommand<SelectionChangedEventArgs> FontHeightChangedCommand => fontHeightChangedCommand ?? (fontHeightChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                param =>
                {
                    Gui.RibbonComboboxFontHeight((ComboBox)param.Source);
                }
            ));
    }
}
