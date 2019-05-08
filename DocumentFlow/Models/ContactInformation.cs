using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class ContactInformation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TypeId { get; set; }
        public virtual ContactInfoType ContactInfoType { get; set; }
        public string Value { get; set; }
    }
}
