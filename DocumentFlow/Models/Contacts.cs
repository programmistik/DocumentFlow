using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }

        public virtual ICollection<ContactInformation> ContactInfos { get; set; }
        public Contact()
        {
            this.ContactInfos = new HashSet<ContactInformation>();
        }
    }

    public class Employee : Contact
    {
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }
        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }
        public virtual Position Position { get; set; }
        public int PositionId { get; set; }

        public bool HeadOfDep { get; set; }
        public bool CanEditContacts { get; set; }

    }

    //public class ExternalContact : Contact
    //{

    //}
}
