using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string SaltValue { get; set; }
        public string HashValue { get; set; }

        //public User()
        //{
        //    this.Trips = new HashSet<Trip>();
        //}
        //public virtual ICollection<Trip> Trips { get; set; }
    }
}
