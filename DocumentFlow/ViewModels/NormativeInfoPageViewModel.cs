using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class NormativeInfoPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private ObservableCollection<Company> companyCollection;
        public ObservableCollection<Company> CompanyCollection { get => companyCollection; set => Set(ref companyCollection, value); }

        private ObservableCollection<Department> departmentCollection;
        public ObservableCollection<Department> DepartmentCollection { get => departmentCollection; set => Set(ref departmentCollection, value); }

        private ObservableCollection<Position> positionCollection;
        public ObservableCollection<Position> PositionCollection { get => positionCollection; set => Set(ref positionCollection, value); }


        public NormativeInfoPageViewModel(INavigationService navigationService, 
                                             IMessageService messageService, 
                                                AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            CompanyCollection = db.Companies.Local;  //new ObservableCollection<Company>(db.Companies);
            DepartmentCollection = db.Departments.Local;  //new ObservableCollection<Department>(db.Departments);
            PositionCollection = db.Positions.Local;  //new ObservableCollection<Position>(db.Positions);
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    db.SaveChanges();
                    navigationService.Navigate<AdminPanelPageView>();

                }
                 ));

    }
}
