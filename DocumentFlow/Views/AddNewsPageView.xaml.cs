using DocumentFlow.Services.WebBrowserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocumentFlow.Views
{
    /// <summary>
    /// Interaction logic for AddNewsPageView.xaml
    /// </summary>
    public partial class AddNewsPageView : UserControl
    {
        public AddNewsPageView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gui.webBrowser = webBrowserEditor;

            Gui.htmlEditor = HtmlEditor1;
            NewPostInitialisation.webeditor = this;
            Gui.newdocument();

            NewPostInitialisation.RibbonComboboxFontsInitialisation();
            NewPostInitialisation.RibbonComboboxFontSizeInitialisation();
        }
    }
}
