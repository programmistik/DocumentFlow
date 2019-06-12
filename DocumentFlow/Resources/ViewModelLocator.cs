using Autofac;
using Autofac.Configuration;
using DocumentFlow.Services;
using DocumentFlow.ViewModels;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentFlow
{
    class ViewModelLocator
    {
        private AppViewModel appViewModel;

        private LogInPageViewModel LogInViewModel;
        private MainDesktopPageViewModel MainDesktopPageViewModel;
        private CalendarPageViewModel CalendarPageViewModel;
        private AddEditEventPageViewModel AddEditEventPageViewModel;
        private GMailPageViewModel GMailPageViewModel;
        private ReadMailPageViewModel ReadMailPageViewModel;
        private SchedulePageViewModel SchedulePageViewModel;
        private DocumentsPageViewModel DocumentsPageViewModel;
        private NewsPageViewModel NewsPageViewModel;
        private SettingsPageViewModel SettingsPageViewModel;
        private ContactsPageViewModel ContactsPageViewModel;
        private ComposeNewMailPageViewModel ComposeNewMailPageViewModel;
        private AdminPanelPageViewModel AdminPanelPageViewModel;
        private AddNewUserPageViewModel AddNewUserPageViewModel;
        private NormativeInfoPageViewModel NormativeInfoPageViewModel;
        private AddNewsPageViewModel AddNewsPageViewModel;
        private NewsListPageViewModel NewsListPageViewModel;
        private ConstantsPageViewModel ConstantsPageViewModel;
        private ChangeMyPassPageViewModel ChangeMyPassPageViewModel;
        private CreateNewContactPageViewModel CreateNewContactPageViewModel;
        private DocPageViewModel DocPageViewModel;

        private INavigationService navigationService;

        public static IContainer Container;

        public ViewModelLocator()
        {
            try
            {
                var config = new ConfigurationBuilder();
                config.AddJsonFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\autofac.json");
                var module = new ConfigurationModule(config.Build());
                var builder = new ContainerBuilder();
                builder.RegisterModule(module);
                Container = builder.Build();

                navigationService = Container.Resolve<INavigationService>();
                appViewModel = Container.Resolve<AppViewModel>();
                //     
                LogInViewModel = Container.Resolve<LogInPageViewModel>();
                MainDesktopPageViewModel = Container.Resolve<MainDesktopPageViewModel>();
                CalendarPageViewModel = Container.Resolve<CalendarPageViewModel>();
                AddEditEventPageViewModel = Container.Resolve<AddEditEventPageViewModel>();
                GMailPageViewModel = Container.Resolve<GMailPageViewModel>();
                ReadMailPageViewModel = Container.Resolve<ReadMailPageViewModel>();
                SchedulePageViewModel = Container.Resolve<SchedulePageViewModel>();
                DocumentsPageViewModel = Container.Resolve<DocumentsPageViewModel>();
                NewsPageViewModel = Container.Resolve<NewsPageViewModel>();
                SettingsPageViewModel = Container.Resolve<SettingsPageViewModel>();
                ContactsPageViewModel = Container.Resolve<ContactsPageViewModel>();
                ComposeNewMailPageViewModel = Container.Resolve<ComposeNewMailPageViewModel>();
                AdminPanelPageViewModel = Container.Resolve<AdminPanelPageViewModel>();
                AddNewUserPageViewModel = Container.Resolve<AddNewUserPageViewModel>();
                NormativeInfoPageViewModel = Container.Resolve<NormativeInfoPageViewModel>();
                AddNewsPageViewModel = Container.Resolve<AddNewsPageViewModel>();
                NewsListPageViewModel = Container.Resolve<NewsListPageViewModel>();
                ConstantsPageViewModel = Container.Resolve<ConstantsPageViewModel>();
                ChangeMyPassPageViewModel = Container.Resolve<ChangeMyPassPageViewModel>();
                CreateNewContactPageViewModel = Container.Resolve<CreateNewContactPageViewModel>();
                DocPageViewModel = Container.Resolve<DocPageViewModel>();

                navigationService.Register<LogInPageView>(LogInViewModel);
                navigationService.Register<MainDesktopPageView>(MainDesktopPageViewModel);
                navigationService.Register<CalendarPageView>(CalendarPageViewModel);
                navigationService.Register<AddEditEventPageView>(AddEditEventPageViewModel);
                navigationService.Register<GMailPageView>(GMailPageViewModel);
                navigationService.Register<ReadMailPageView>(ReadMailPageViewModel);
                navigationService.Register<SchedulePageView>(SchedulePageViewModel);
                navigationService.Register<DocumentsPageView>(DocumentsPageViewModel);
                navigationService.Register<NewsPageView>(NewsPageViewModel);
                navigationService.Register<SettingsPageView>(SettingsPageViewModel);
                navigationService.Register<ContactsPageView>(ContactsPageViewModel);
                navigationService.Register<ComposeNewMailPageView>(ComposeNewMailPageViewModel);
                navigationService.Register<AdminPanelPageView>(AdminPanelPageViewModel);
                navigationService.Register<AddNewUserPageView>(AddNewUserPageViewModel);
                navigationService.Register<NormativeInfoPageView>(NormativeInfoPageViewModel);
                navigationService.Register<AddNewsPageView>(AddNewsPageViewModel);
                navigationService.Register<NewsListPageView>(NewsListPageViewModel);
                navigationService.Register<ConstantsPageView>(ConstantsPageViewModel);
                navigationService.Register<ChangeMyPassPageView>(ChangeMyPassPageViewModel);
                navigationService.Register<CreateNewContactPageView>(CreateNewContactPageViewModel);
                navigationService.Register<DocPageView>(DocPageViewModel);

                navigationService.Navigate<LogInPageView>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ViewModelBase GetAppViewModel()
        {
            return appViewModel;
        }
    }
}
