using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentFlow.ViewModels
{
    public class ConstantsPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private string docPath;
        public string DocPath { get => docPath; set => Set(ref docPath, value); }

        private string filePath;
        public string FilePath { get => filePath; set => Set(ref filePath, value); }

        public ConstantsPageViewModel(INavigationService navigationService,
                                             IMessageService messageService,
                                                AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
        }

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(
        () =>
        {
            var s = db.Constants.FirstOrDefault();
            DocPath = s.DocPath;
            FilePath = s.FilePath;
        }));

        private RelayCommand docPathCommand;
        public RelayCommand DocPathCommand => docPathCommand ?? (docPathCommand = new RelayCommand(
        () =>
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    DocPath = fbd.SelectedPath;
                }
            }
        }));

        private RelayCommand filePathCommand;
        public RelayCommand FilePathCommand => filePathCommand ?? (filePathCommand = new RelayCommand(
        () =>
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    FilePath = fbd.SelectedPath;
                }
            }
        }));

        private RelayCommand okCommand;
        public RelayCommand OkCommand => okCommand ?? (okCommand = new RelayCommand(
        async () =>
        {
            var s = db.Constants.FirstOrDefault();
            s.DocPath = DocPath;
            s.FilePath = FilePath;
            await db.SaveChangesAsync();
            navigationService.Navigate<AdminPanelPageView>();
        }));
    }
}
