using ListManagement.models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ListManagement.Standard.utilities
{
    public class ItemJsonConverter : JsonCreationConverter<Item>
    {
        protected override Item Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["Deadline"] != null || jObject["deadline"] != null)
            {
                return new ToDo();
            }
            else if (jObject["Start"] != null || jObject["start"] != null)
            {
                return new Appointment();
            }
            else
            {
                return new Item();
            }
        }
    }
}