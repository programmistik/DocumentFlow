using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class MyFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUri { get; set; }

        public int DocId { get; set; }
        public virtual Document Doc { get; set; }
    }
}
