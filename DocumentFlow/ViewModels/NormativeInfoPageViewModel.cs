using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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

            LoadTablesAsync();

            

            //CompanyCollection = db.Companies.Local;  //new ObservableCollection<Company>(db.Companies);
            //DepartmentCollection = db.Departments.Local;  //new ObservableCollection<Department>(db.Departments);
            //PositionCollection = db.Positions.Local;  //new ObservableCollection<Position>(db.Positions);
        }

        private async void LoadTablesAsync()
        {
            
           // CollectionViewSource CompanyCollection = new CollectionViewSource(); //((CollectionViewSource)(FindResource("CompanyCollection")));
            await db.Companies.LoadAsync();
            CompanyCollection = db.Companies.Local;
            await db.Departments.LoadAsync();
            DepartmentCollection = db.Departments.Local;
            await db.Positions.LoadAsync();
            PositionCollection = db.Positions.Local;
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                async () =>
                {
                   
                    await db.SaveChangesAsync();
                    navigationService.Navigate<AdminPanelPageView>();

                }
                 ));

    }
}
