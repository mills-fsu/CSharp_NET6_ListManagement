using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManagement.models
{
    public class Appointment : Item
    {
        private DateTimeOffset boundDate;
        public DateTimeOffset BoundDate
        {
            get
            {
                return boundDate;
            }
            set
            {
                boundDate = value;
                Start = boundDate.Date;
            }
        }

        private DateTimeOffset boundDate1;
        public DateTimeOffset BoundDate1
        {
            get
            {
                return boundDate1;
            }
            set
            {
                boundDate1 = value;
                End = boundDate1.Date;
            }
        }


        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<string> Attendees { get; set; }

        public Appointment()
        {
            Attendees = new List<string>();
        }

        public override string ToString()
        {
            return $"{Name} {Description} Start: {Start} End: {End} Priority {Priority}";
        }
    }
}