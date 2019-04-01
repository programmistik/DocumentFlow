using DocumentFlow.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentFlow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Set localization
            // by default app will use windows culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("us");

            var locator = new ViewModelLocator();

            var window = new AppWindow();
            window.DataContext = locator.GetAppViewModel();
            window.ShowDialog();
        }
    }
}
