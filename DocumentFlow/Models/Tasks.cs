using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class TaskProcess
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int? StartUserId { get; set; }
        public virtual User StartUser { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public int? TaskUserId { get; set; }
        public virtual Employee TaskUser { get; set; }
        //  public DateTime Deadline { get; set; }
        public int? StateId { get; set; }
        public virtual DocumentState State { get; set; }
        public string Comment { get; set; }
        public DateTime? StateDate { get; set; }

        public int DocId { get; set; }
        public virtual Document Doc { get; set; }
    }
}
