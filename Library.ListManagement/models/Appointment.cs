using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManagement.models
{
    public class Appointment : Item
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<string> Attendees { get; set; }

        public Appointment(string name, string desc, DateTime begin, DateTime end, List<String> attendants)
        {
            this.Name = name;
            this.Description = desc;
            this.Start = begin;
            this.End = end;
            this.Attendees = attendants;
        }

        public override string ToString()
        {
            return $"{Name} {Description} From {Start} to {End}";
        }
    }
}