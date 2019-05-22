using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class NewsPost
    {
        public int Id { get; set; }
        public string PostContent { get; set; }
        public string PostHeader { get; set; }
        public DateTime PostEndDate { get; set; }
    }
}
