using DocumentFlow.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentFlow.ModalWindows
{
    public class AddContactInfoViewModel : ViewModelBase
    {
        private ObservableCollection<ContactInfoType> infoCollection;
        public ObservableCollection<ContactInfoType> InfoCollection { get => infoCollection; set => Set(ref infoCollection, value); }

        private ContactInfoType selectedInfo;
        public ContactInfoType SelectedInfo { get => selectedInfo; set => Set(ref selectedInfo, value); }

        private string inputValue;
        public string InputValue { get => inputValue; set => Set(ref inputValue, value); }

        private string title;
        public string Title { get => title; set => Set(ref title, value); }

        public AddContactInfoViewModel(List<ContactInfoType> lst, ContactInfoType typ, string val)
        {
            InfoCollection = new ObservableCollection<ContactInfoType>(lst);
            SelectedInfo = typ;
            InputValue = val;
            if (val == "")
            {
                Title = "Add new contact information";
            }
            else
                Title = "Edit contact information";
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c < '0' || c > '9') && c != '+' && c != '-' && c != ' ')
                    return false;
            }
            return true;
        }

        bool EmailValidation(string str)
        {
            string email = str;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private RelayCommand<AddContactInfoWindow> okButton_Click;
        public RelayCommand<AddContactInfoWindow> OkButton_Click => okButton_Click ?? (okButton_Click = new RelayCommand<AddContactInfoWindow>(
                param =>
                {
                    if (SelectedInfo.InfoType == "Phone" || SelectedInfo.InfoType == "Mobile")
                    {
                        if (IsDigitsOnly(InputValue) == false)
                        {
                            MessageBox.Show("Invalid phone");
                        }
                        else
                        {
                            param.Hide();
                        }

                    }
                    else if (SelectedInfo.InfoType == "e-mail")
                    {
                        if (EmailValidation(InputValue) == false)
                        {
                            MessageBox.Show("Invalid e-mail format");
                        }
                        else
                        {
                            param.Hide();
                        }

                    }
                    else
                        param.Hide();
                }
            ));

        private RelayCommand<AddContactInfoWindow> cancelButton_Click;
        public RelayCommand<AddContactInfoWindow> CancelButton_Click => cancelButton_Click ?? (cancelButton_Click = new RelayCommand<AddContactInfoWindow>(
                param =>
                {
                    param.Hide();
                    InputValue = null;
                }
            ));

    }
}
