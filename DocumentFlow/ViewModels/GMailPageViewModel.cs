using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
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
        public GMailPageViewModel(INavigationService navigationService, IMessageService messageService, AppDbContext db, IGoogleService googleService)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            this.googleService = googleService;
            GMailService = googleService.getGMailService();
           
        }

        private RelayCommand receiveMailCommand;
        public RelayCommand ReceiveMailCommand => receiveMailCommand ?? (receiveMailCommand = new RelayCommand(
                () =>
                {
                    // Define parameters of request.
                    var request = GMailService.Users.Labels.List("me");

                    // List labels.
                    IList<Label> labels = request.Execute().Labels;
                    
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
                }
                 ));
    }
}
