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
    /// Interaction logic for ContactsPageView.xaml
    /// </summary>
    public partial class ContactsPageView : UserControl
    {
        public ContactsPageView()
        {
            InitializeComponent();
        }

        //private void ContactsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void ContactsGrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    List<ContactsGridTable> result = new List<ContactsGridTable>(3);
        //    result.Add(new ContactsGridTable(1, "Sanan Yusibov", "STEP IT", "+994557077571", "+994125991159"));
        //    result.Add(new ContactsGridTable(2, "Михеев Иосиф Федорович", "R.I.S.K Company", "+994557366833", "+994124366583"));
        //    result.Add(new ContactsGridTable(3, "Александр Иванов", "Socar", "+994557366833", "+994124366583"));
        //    result.Add(new ContactsGridTable(4, "Максим Иванов", "BP Company", "+994557366833", "+994124366583"));
        //    result.Add(new ContactsGridTable(5, "Федор Тестов", "Test Company", "+994557366833", "+994124366583"));
        //    ContactsGrid.ItemsSource = result;
        //}

        //class ContactsGridTable
        //{
        //    public ContactsGridTable(int Id, string Name, string Company, string Phone, string Company_Phone)
        //    {
        //        this.Id = Id;
        //        this.Name = Name;
        //        this.Company = Company;
        //        this.Phone = Phone;
        //        this.Company_Phone = Company_Phone;
        //    }
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //    public string Company { get; set; }
        //    public string Phone { get; set; }
        //    public string Company_Phone { get; set; }

        //}

        //private void ContactsGrid_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    ContactsGridTable path = ContactsGrid.SelectedItem as ContactsGridTable;
        //    MessageBox.Show(" ID: " + path.Id + "\n Имя: " + path.Name +
        //        "\n Компания: " + path.Company + "\n Мобильный номер представителя: " + path.Phone
        //        + "\n Номер компании: " + path.Company_Phone);
        //}
    }
}
