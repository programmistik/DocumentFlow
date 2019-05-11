using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class History
    {
        public int Id { get; set; }
        public string WhoEdited { get; set; }
        public DateTime EditionData { get; set; }
        public string ClassName { get; set; }
        public int ObjectId { get; set; }
        public string ObjectJson { get; set; }
    }
}
