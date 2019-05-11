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
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public int TypeId { get; set; }
        public virtual ContactInfoType ContactInfoType { get; set; }
        public string Value { get; set; }
    }
}
