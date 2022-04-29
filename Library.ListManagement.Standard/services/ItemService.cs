using Library.ListManagement.helpers;
using Library.ListManagement.Standard.utilities;
using ListManagement.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ListManagement.services
{
    public class ItemService
    {
        public bool searched = false;
        public string lastSearched = "";
        private ObservableCollection<Item> items;
        private List<Item> fullItems = new List<Item>();
        private ListNavigator<Item> listNav;
        private string persistencePath;
        private JsonSerializerSettings serializerSettings
            = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        static private ItemService instance;

        public bool ShowComplete { get; set; }
        public ObservableCollection<Item> Items
        {
            get
            {
                return items;
            }

            set
            {

            }
        }


        public string Query { get; set; }

        public IEnumerable<Item> FilteredItems
        {
            get
            {
                var incompleteItems = Items.Where(i =>
                (!ShowComplete && !((i as ToDo)?.IsCompleted ?? true)) //incomplete only
                || ShowComplete);
                //show complete (all)

                var searchResults = incompleteItems.Where(i => string.IsNullOrWhiteSpace(Query)
                //there is no query
                || (i?.Name?.ToUpper()?.Contains(Query.ToUpper()) ?? false)
                //i is any item and its name contains the query
                || (i?.Description?.ToUpper()?.Contains(Query.ToUpper()) ?? false)
                //or i is any item and its description contains the query
                || ((i as Appointment)?.Attendees?.Select(t => t.ToUpper())?.Contains(Query.ToUpper()) ?? false));
                //or i is an appointment and has the query in the attendees list
                return searchResults;
            }
        }

        public static ItemService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemService();
                }
                return instance;
            }
        }

        private ItemService()
        {
            items = new ObservableCollection<Item>();
            persistencePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\SaveData.json";


            try
            {
                LoadFromServer();
            }
            catch (Exception)
            {
                LoadFromDisk();
            }
            
        }

        private void LoadFromServer()
        {
            items.Clear();
            var payload = JsonConvert.DeserializeObject<List<Item>>(new WebRequestHandler().Get("http://localhost/ListManagementAPI/ToDo").Result);
            payload.ForEach(items.Add);
            /*
            foreach(var item in payload)
            {
                bool exists = false;
                foreach(var j in items)
                {
                    if (item.Id == j.Id)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    items.Add(item);
                }
            }
            */
            listNav = new ListNavigator<Item>(FilteredItems, 2);
        }

        private void LoadFromDisk()
        {
            if (File.Exists(persistencePath))
            {
                try
                {
                    var state = File.ReadAllText(persistencePath);
                    if (state != null)
                    {
                        items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(state, serializerSettings) ?? new ObservableCollection<Item>();
                    }
                }
                catch (Exception e)
                {
                    File.Delete(persistencePath);
                    items = new ObservableCollection<Item>();
                }
            }
        }

        public void Add(Item i)
        {
            if (i.Id <= 0)
            {
                i.Id = NextId;
            }
            
            items.Add(i);
            
        }

        public void Remove(Item i)
        {
            items.Remove(i);
            /*
            if (i is ToDo)
            {
                JsonConvert.DeserializeObject<ToDo>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/ToDo/Remove", i).Result);
            }
            if (i is Appointment)
            {
                JsonConvert.DeserializeObject<Appointment>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/Appointment/Remove", i).Result);
            }
            */
        }

        public void Save()
        {
            //save to disk
            var listJson = JsonConvert.SerializeObject(Items, serializerSettings);
            if (File.Exists(persistencePath))
            {
                File.Delete(persistencePath);
            }
            File.WriteAllText(persistencePath, listJson);


            foreach (var i in items)
            {
                if (i is ToDo)
                {
                    JsonConvert.DeserializeObject<ToDo>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/ToDo/AddOrUpdate", i).Result);
                }
                if (i is Appointment)
                {

                    JsonConvert.DeserializeObject<Appointment>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/Appointment/AddOrUpdate", i).Result);
                }
            }



                //save to server

            var payload = JsonConvert.DeserializeObject<List<Item>>(new WebRequestHandler().Get("http://localhost/ListManagementAPI/ToDo").Result);
            foreach (var i in payload)
            {
                bool exists = false;
                foreach(var j in items)
                {
                    if(i.Id == j.Id)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    if (i is ToDo)
                    {
                        JsonConvert.DeserializeObject<ToDo>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/ToDo/Remove", i).Result);
                    }
                    if (i is Appointment)
                    {
                        JsonConvert.DeserializeObject<Appointment>(new WebRequestHandler().Post("http://localhost/ListManagementAPI/Appointment/Remove", i).Result);
                    }
                }
                else
                {
                    
                }

            }
            

        }

        public void Load() 
        {
            try
            {
                
                LoadFromServer();
            }
            catch (Exception)
            {
                LoadFromDisk();
            }
        }

        public Dictionary<object, Item> GetPage()
        {
            var page = listNav.GetCurrentPage();
            if (listNav.HasNextPage)
            {
                page.Add("N", new Item { Name = "Next" });
            }
            if (listNav.HasPreviousPage)
            {
                page.Add("P", new Item { Name = "Previous" });
            }
            return page;
        }

        public Dictionary<object, Item> NextPage()
        {
            return listNav.GoForward();
        }

        public Dictionary<object, Item> PreviousPage()
        {
            return listNav.GoBackward();
        }

        public int NextId
        {
            get
            {
                if (Items.Any())
                {
                    return Items.Select(i => i.Id).Max() + 1;
                }
                return 1;
            }
        }
        public void Sort()
        {
            var items1 = new ObservableCollection<Item>(items.OrderBy(i => i.Priority));
            for (int i = 0; i < items.Count; i++)
            {
                items.Move(items.IndexOf(items1[i]), i);
            }

        }

        public void Search(string query) 
        {
            string stringToSearch = query;
            stringToSearch = stringToSearch.ToUpper();

            if (!searched)
            {
                searched = true;
                lastSearched = query;

                //reset fullitems
                for (int i = 0; i < fullItems.Count; i++)
                {
                    fullItems.Remove(fullItems[i]);
                }
                for (int i = 0; i < items.Count; i++)
                {
                    fullItems.Add(items[i]);
                }
                var Found = new ObservableCollection<Item>();
                var results = from item in Items
                              where item.Name.ToUpper().Contains(stringToSearch) || item.Description.ToUpper().Contains(stringToSearch)
                              || ((item as Appointment)?.Attendees?.Any(a => a.ToUpper().Contains(stringToSearch)) ?? false)
                              select item;
                foreach (var res in results)
                {
                    Found.Add(res);
                }
                items.Clear();
                for (int i = 0; i < Found.Count; i++)
                {
                    items.Add(Found[i]);
                }

            }
            else if(query != lastSearched)
            {
                items.Clear();
                for (int i = 0; i < fullItems.Count; i++)
                {
                    items.Add(fullItems[i]);
                }

                fullItems.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    fullItems.Add(items[i]);
                }
                var Found = new ObservableCollection<Item>();
                var results = from item in items
                              where item.Name.ToUpper().Contains(stringToSearch) || item.Description.ToUpper().Contains(stringToSearch)
                              || ((item as Appointment)?.Attendees?.Any(a => a.ToUpper().Contains(stringToSearch)) ?? false)
                              select item;
                foreach (var res in results)
                {
                    Found.Add(res);
                }
                items.Clear();
                for (int i = 0; i < Found.Count; i++)
                {
                    items.Add(Found[i]);
                }
            }
        }

    }
}