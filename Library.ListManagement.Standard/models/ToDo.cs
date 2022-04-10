using ListManagement.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManagement.models
{
    public class ToDo : Item
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
                Deadline = boundDate.Date;
            }
        }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"{Name} | {Description} | Due: {(Deadline.ToString("dd/MM/yyyy"))} | Completed: {IsCompleted} | Priority: {Priority}";
        }
    }
}