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

                navigationService.Register<LogInPageView>(LogInViewModel);
                navigationService.Register<MainDesktopPageView>(MainDesktopPageViewModel);
                navigationService.Register<CalendarPageView>(CalendarPageViewModel);
                navigationService.Register<AddEditEventPageView>(AddEditEventPageViewModel);
                navigationService.Register<GMailPageView>(GMailPageViewModel);
                navigationService.Register<ReadMailPageView>(ReadMailPageViewModel);

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
