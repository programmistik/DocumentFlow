using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class GMailPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;
        private readonly IGoogleService googleService;
        private GmailService GMailService;

        private ObservableCollection<GoogleMessage> inboxList;
        public ObservableCollection<GoogleMessage> InboxList { get => inboxList; set => Set(ref inboxList, value); }

        public GMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GMailService = googleService.getGMailService();
            InboxList = new ObservableCollection<GoogleMessage>();
           
        }

        private byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(string.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }

        private RelayCommand receiveMailCommand;
        public RelayCommand ReceiveMailCommand => receiveMailCommand ?? (receiveMailCommand = new RelayCommand(
                () =>
                {
                    // Define parameters of request.
                    //var request = GMailService.Users.Labels.List("me");

                    //// List labels.
                    //IList<Label> labels = request.Execute().Labels;

                    //if (labels != null && labels.Count > 0)
                    //{
                    //    foreach (var labelItem in labels)
                    //    {
                    //        Console.WriteLine("{0}", labelItem.Name);
                    //    }
                    //}
                    //else
                    //{
                    //    Console.WriteLine("No labels found.");
                    //}
                    //Console.Read();

                    var inboxlistRequest = GMailService.Users.Messages.List("me");
                    inboxlistRequest.LabelIds = "INBOX";
                    inboxlistRequest.IncludeSpamTrash = false;
                    //get our emails
                    var emailListResponse = inboxlistRequest.Execute();

                    if (emailListResponse != null && emailListResponse.Messages != null)
                    {
                        //loop through each email and get what fields you want...
                        foreach (var email in emailListResponse.Messages)
                        {

                            var emailInfoRequest = GMailService.Users.Messages.Get("me", email.Id);
                            emailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                            var emailInfoResponse = emailInfoRequest.Execute();

                            if (emailInfoResponse != null)
                            {
                                //InboxList.Add(emailInfoResponse);
                                var newMail = new GoogleMessage();
                                newMail.FullMessage = emailInfoResponse;

                                var from = "";
                                var date = "";
                                var subject = "";

                                //loop through the headers to get from,date,subject, body 
                                foreach (var mParts in emailInfoResponse.Payload.Headers)
                                {
                                    if (mParts.Name == "Date")
                                    {
                                        date = mParts.Value;
                                        newMail.Date = date;
                                    }
                                    else if (mParts.Name == "From")
                                    {
                                        from = mParts.Value;
                                        newMail.From = from;
                                    }
                                    else if (mParts.Name == "Subject")
                                    {
                                        subject = mParts.Value;
                                        newMail.Subject = subject;
                                    }

                                    if (date != "" && from != "")
                                    {
                                        if (emailInfoResponse.Payload.Parts != null)
                                        {
                                            foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                                            {
                                                if (p.MimeType == "text/html")
                                                {
                                                    byte[] data = FromBase64ForUrlString(p.Body.Data);
                                                    string decodedString = Encoding.UTF8.GetString(data);
                                                    newMail.Html = decodedString;
                                                    InboxList.Add(newMail);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
                 ));

        private RelayCommand<GoogleMessage> readMailCommand;
        public RelayCommand<GoogleMessage> ReadMailCommand => readMailCommand ?? (readMailCommand = new RelayCommand<GoogleMessage>(
                param =>
                {
                    Messenger.Default.Send(new NotificationMessage<GoogleMessage>(param, "MailToRead"));
                    navigationService.Navigate<ReadMailPageView>();
                }
                 ));
    }
}
