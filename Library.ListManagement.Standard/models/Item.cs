using Library.ListManagement.Standard.utilities;
using ListManagement.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManagement.models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item : IItem
    {
        private string _name;
        private string _description;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }
        public string Description {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            } 
        }
        public int Priority { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return $"{Name} {Description}";
        }
    }
}