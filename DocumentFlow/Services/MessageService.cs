using BespokeFusion;
using DocumentFlow.ModalWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DocumentFlow.Services
{
    class MessageService : IMessageService
    {
        public void SelectableInfo(string text, string title = "Info", string msgUri = "")
        {
            var res = SelectableMessageBox.Show(title, text, MessageBoxButton.OK, MessageBoxImage.Information, msgUri);
        }

        public void ShowError(string text, string title = "Error")
        {
            MaterialMessageBox.ShowError(text);
        }

        public void ShowInfo(string text, string title = "Info")
        {
            MaterialMessageBox.Show(text, title);
        }

        public bool ShowYesNo(string text, string title = "Your answer")
        {
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = text, Foreground = Brushes.Black },
                TxtTitle = { Text = title, Foreground = Brushes.White },
                BtnOk = { Content = "OK" },
                MainContentControl = { Background = Brushes.White },
                TitleBackgroundPanel = { Background = Brushes.BlueViolet },

                BorderBrush = Brushes.BlueViolet
            };

            msg.Show();

            var result = msg.Result;
            return result == MessageBoxResult.OK;
        }
    }
}
