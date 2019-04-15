using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DocumentFlow.Models
{
    public class GoogleEvent
    {
        public DateTime? Craeded { get; set; }
        public string CreatorEmail { get; set; }
        public string EventSummary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? Updated { get; set; }
        public List<Attendee> Attendees { get; set; }

    }

    public class Attendee
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }

    public class GoogleColors
    {
        private string ColorId { get; set; }
        private string ColorName { get; set; }
        private Brush ColorHex { get; set; }

        public GoogleColors(string ColorId, string ColorName, string ColorHex)
        {
            this.ColorId = ColorId;
            this.ColorName = ColorName;
            Brush br = new SolidColorBrush();
            br = (SolidColorBrush)(new BrushConverter().ConvertFrom(ColorHex));
            this.ColorHex = br;
        }
    }
}
