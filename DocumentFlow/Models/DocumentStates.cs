using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class DocumentState
    {
        public int Id { get; set; }
        public string DocStateName { get; set; }

        [NotMapped]
        public bool IsSelectable { get; set; }
    }
}
