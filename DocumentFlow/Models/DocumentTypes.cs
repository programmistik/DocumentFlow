using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string DocTypeName { get; set; }
        [MaxLength(3)]
        public string DocTypeAcc { get; set; }
    }
}
