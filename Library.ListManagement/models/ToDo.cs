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
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo(string name, string desc, DateTime dead)
        {
            this.Name = name;
            this.Description = desc;
            this.Deadline = dead;
            IsCompleted = false;
        }

        public override string ToString()
        {
            return $"{Name} {Description} Completed: {IsCompleted}";
        }
    }
}