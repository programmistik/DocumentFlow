using DocumentFlow.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentFlow.ModalWindows
{
    public class AddEditProcessViewModel : ViewModelBase
    {
        private Department dept;
        public Department Dept { get => dept; set => Set(ref dept, value); }

        private ObservableCollection<Department> deptCollection;
        public ObservableCollection<Department> DeptCollection { get => deptCollection; set => Set(ref deptCollection, value); }

        private Employee emp;
        public Employee Emp { get => emp; set => Set(ref emp, value); }

        private ObservableCollection<Employee> empCollection;
        public ObservableCollection<Employee> EmpCollection { get => empCollection; set => Set(ref empCollection, value); }

        private DocumentState state;
        public DocumentState State { get => state; set => Set(ref state, value); }

        private ObservableCollection<DocumentState> stateCollection;
        public ObservableCollection<DocumentState> StateCollection { get => stateCollection; set => Set(ref stateCollection, value); }

        private string comment;
        public string Comment { get => comment; set => Set(ref comment, value); }

        private string title;
        public string Title { get => title; set => Set(ref title, value); }

        private List<Employee> AllEmployees { get; set; }
        public TaskProcess Process { get; set; }

        public AddEditProcessViewModel(TaskProcess taskProcess, List<Department> DeptCollection,
                List<Employee> EmpCollection, List<DocumentState> StateCollection)
        {
            if (taskProcess == null)
            {
                Process = taskProcess;
                Title = "Add new process";
                this.DeptCollection = new ObservableCollection<Department>(DeptCollection);
                AllEmployees = EmpCollection;
                this.EmpCollection = new ObservableCollection<Employee>(EmpCollection);
                State = StateCollection.Where(s => s.DocStateName == "New").Single();
                State.IsSelectable = true;
                this.StateCollection = new ObservableCollection<DocumentState>(StateCollection);
            }
        }

        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand => selectionChangedCommand ?? (selectionChangedCommand = new RelayCommand(
                () =>
                {
                    if (Dept != null)
                        EmpCollection = new ObservableCollection<Employee>(AllEmployees.Where(x => x.DepartmentId == Dept.Id));
                }
            ));

        private RelayCommand<Employee> emp_SelectionChangedCommand;
        public RelayCommand<Employee> Emp_SelectionChangedCommand => emp_SelectionChangedCommand ?? (emp_SelectionChangedCommand = new RelayCommand<Employee>(
               param =>
               {
                   if (param != null)
                       Dept = param.Department;
               }
            ));

        private RelayCommand<AddEditProcessWindow> okCommand;
        public RelayCommand<AddEditProcessWindow> OkCommand => okCommand ?? (okCommand = new RelayCommand<AddEditProcessWindow>(
        param =>
        {
            var ErrorMsg = new StringBuilder("This fields can't be empty:\n");
            var ok = true;
            if (Dept == null)
            {
                ok = false;
                ErrorMsg.Append("- Department\n");
            }
            if (Emp == null)
            {
                ok = false;
                ErrorMsg.Append("- Responce person\n");
            }
            if (State == null)
            {
                ok = false;
                ErrorMsg.Append("- State\n");
            }

            if (ok == false)
            {
                ErrorMsg.Append("Changes not saved.");
                MessageBox.Show(ErrorMsg.ToString());
                return;
            }

            if (Process == null)
                Process = new TaskProcess
                {
                    Department = Dept,
                    TaskUser = Emp,
                    State = State,
                    Comment = Comment
                };
            param.Hide();
        }));

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(
        () =>
        {

        }));
    }
}
