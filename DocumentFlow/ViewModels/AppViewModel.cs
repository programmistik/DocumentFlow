using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFlow.Services;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;

namespace DocumentFlow.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private ViewModelBase currentPage;
        public ViewModelBase CurrentPage { get => currentPage; set => Set(ref currentPage, value); }

        private string applicationTitle;
        public string ApplicationTitle { get => applicationTitle; set => Set(ref applicationTitle, value); }


        private readonly INavigationService navigation;

        public AppViewModel(INavigationService navigation)
        {
            this.navigation = navigation;

            //Messenger.Default.Register<ViewModelBase>(this, viewModel => CurrentPage = viewModel);
            Messenger.Default.Register<ViewModelBase>(this, viewModel =>
            {
                CurrentPage = viewModel;
                var str = viewModel.ToString();
                if (viewModel is LogInPageViewModel)
                    ApplicationTitle = Properties.Resources.LogIn;
               
            }
            );
        }

        private RelayCommand<Type> navigateCommand;
        public RelayCommand<Type> NavigateCommand
        {
            get => navigateCommand ?? (navigateCommand = new RelayCommand<Type>(
                param =>
                {
                    navigation.Navigate(param);
                }
            ));
        }


    }
}
