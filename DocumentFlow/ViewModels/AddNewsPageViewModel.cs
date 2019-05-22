using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Services.WebBrowserServices;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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

        private DateTime untilDate;
        public DateTime UntilDate { get => untilDate; set => Set(ref untilDate, value); }

        private string header;
        public string MyPostHeader { get => header; set => Set(ref header, value); }

        private NewsPost currentPost;
        public NewsPost CurrentPost { get => currentPost; set => Set(ref currentPost, value); }


        public AddNewsPageViewModel(INavigationService navigationService,
                                            IMessageService messageService,
                                               AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;
            UntilDate = DateTime.Today;
            MyPostHeader = "Header...";

            Messenger.Default.Register<NotificationMessage<NewsPost>>(this, OnHitIt);
        }

        private void OnHitIt(NotificationMessage<NewsPost> pst)
        {
            if (pst.Notification == "EditPost")
            {
                CurrentPost = pst.Content;
                PostContent = CurrentPost.PostContent;
                MyPostHeader = CurrentPost.PostHeader;
                UntilDate = CurrentPost.PostEndDate;
            }
            
        }

        private RelayCommand<AddNewsPageView> loadedCommand;
        public RelayCommand<AddNewsPageView> LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand<AddNewsPageView>(
        param =>
        {
            Gui.webBrowser = param.webBrowserEditor;

            Gui.htmlEditor = param.HtmlEditor1;
            NewPostInitialisation.webeditor = param;
            Gui.webBrowser.newWb(CurrentPost.PostContent);

            NewPostInitialisation.RibbonComboboxFontsInitialisation();
            NewPostInitialisation.RibbonComboboxFontSizeInitialisation();

        }));

        #region NavigationCommands

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    CurrentPost = null;
                    PostContent = MyPostHeader = "";
                    UntilDate = DateTime.Today;

                    navigationService.Navigate<NewsListPageView>();

                }
                 ));

        private RelayCommand okCommand;
        public RelayCommand OkCommand => okCommand ?? (okCommand = new RelayCommand(
                async () =>
                {
                    if (string.IsNullOrEmpty(MyPostHeader) || untilDate == null)
                        messageService.ShowError("Please, fill header and date.");
                    else
                    {
                        if (CurrentPost == null) // add new
                        {
                            dynamic doc = Gui.webBrowser.doc;
                            var htmlText = doc.documentElement.InnerHtml;

                            var newPost = new NewsPost
                            {
                                PostContent = htmlText,
                                PostEndDate = UntilDate,
                                PostHeader = MyPostHeader
                            };

                            db.NewsPosts.Add(newPost);
                            
                        }
                        else // edit current
                        {
                            dynamic doc = Gui.webBrowser.doc;
                            var htmlText = doc.documentElement.InnerHtml;

                            CurrentPost.PostContent = htmlText;
                            CurrentPost.PostEndDate = UntilDate;
                            CurrentPost.PostHeader = MyPostHeader;
                        }
                        await db.SaveChangesAsync();
                        navigationService.Navigate<NewsListPageView>();

                        CurrentPost = null;
                        PostContent = "";
                        MyPostHeader = "Header...";
                        UntilDate = DateTime.Today;
                    }

                }
                 ));

        #endregion

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
