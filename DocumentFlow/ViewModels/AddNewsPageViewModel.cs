using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Services.WebBrowserServices;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DocumentFlow.ViewModels
{
    public class AddNewsPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private string postContent;
        public string PostContent { get => postContent; set => Set(ref postContent, value); }

        

        public AddNewsPageViewModel(INavigationService navigationService,
                                            IMessageService messageService,
                                               AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    navigationService.Navigate<AdminPanelPageView>();

                }
                 ));

        private RelayCommand okCommand;
        public RelayCommand OkCommand => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    dynamic doc = Gui.webBrowser.doc;
                    var htmlText = doc.documentElement.InnerHtml;

                    var newPost = new NewsPost
                    {
                        PostContent = htmlText
                    };

                    db.NewsPosts.Add(newPost);
                    db.SaveChanges();
                    navigationService.Navigate<AdminPanelPageView>();

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
