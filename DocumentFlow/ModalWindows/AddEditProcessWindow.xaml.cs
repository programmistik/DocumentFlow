using DocumentFlow.Models;
using GalaSoft.MvvmLight.CommandWpf;
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
using System.Windows.Shapes;

namespace DocumentFlow.ModalWindows
{
    /// <summary>
    /// Interaction logic for AddEditProcessWindow.xaml
    /// </summary>
    public partial class AddEditProcessWindow : Window
    {

        public AddEditProcessWindow(TaskProcess taskProcess, List<Department> DeptCollection, List<Employee> EmpCollection, List<DocumentState> StateCollection)
        {
            InitializeComponent();
            DataContext = new AddEditProcessViewModel(taskProcess, DeptCollection, EmpCollection, StateCollection);
        }

        
    }
}
