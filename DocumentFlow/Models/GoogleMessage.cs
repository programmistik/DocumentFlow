using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class GoogleMessage
    {
        public string From { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public Message FullMessage { get; set; }
    }
}
