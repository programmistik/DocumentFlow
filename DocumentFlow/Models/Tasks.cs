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
        public User StartUser { get; set; }
        public Department Department { get; set; }
        public Employee TaskUser { get; set; }
        public DateTime Deadline { get; set; }
        public DocumentState State { get; set; }
        public string Comment { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
