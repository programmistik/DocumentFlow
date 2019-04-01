using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Services
{
    public interface IMessageService
    {
        void ShowInfo(string text, string title = "Info");
        void ShowError(string text, string title = "Error");
        bool ShowYesNo(string text, string title = "Your answer");
    }
}
