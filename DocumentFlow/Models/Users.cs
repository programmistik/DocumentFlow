using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DocumentFlow.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } // google account login or e-mail address
        public string GoogleAccount { get; set; }
        //public string Name { get; set; }
        //public string Surname { get; set; }
        //public string Photo { get; set; }
      //  public string PrimaryEmail { get; set; }
        public string SaltValue { get; set; }
        public string HashValue { get; set; }        
        public bool IsActive { get; set; }
        //public virtual Company Company { get; set; }
        //public int CompanyId { get; set; }
        //public virtual Department Department { get; set; }
        //public int DepartmentId { get; set; }
        //public virtual Position Position { get; set; }
        //public int PositionId { get; set; }


        //public virtual ICollection<ContactInformation> Contacts { get; set; }
        //public User()
        //{
        //    this.Contacts = new HashSet<ContactInformation>();
        //}

        public override string ToString()
        {
            var entity = new AppDbContext();
            entity.Employees.Load();
            var coll = entity.Employees.Local;
            var emp = coll.Where(e => e.User.GoogleAccount == this.GoogleAccount).Single();
            return $"{GoogleAccount} \n{emp.Name} {emp.Surname} (Dep:{emp.Department.DepartmentName}, Pos:{emp.Position.PositionName})";

        }
    }
}
