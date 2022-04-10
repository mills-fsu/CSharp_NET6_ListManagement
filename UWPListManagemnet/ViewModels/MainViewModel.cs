using ListManagement.models;
using ListManagement.services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UWPListManagement.ViewModels
{
    public class MainViewModel
    {
        private ItemService itemService = ItemService.Current;

        public ObservableCollection<Item> Items
        {
            get
            {
                return itemService.Items;
            }
        }

        public Item SelectedItem
        {
            get; set;
        }

        public String Query
        { get; set; }

        public void Add(Item item)
        {
            itemService.Add(item);
        }
        public void Remove(Item item)
        {
            itemService.Remove(item);
        }

        private void Load(string path)
        {
            MainViewModel mvm;
            if (File.Exists(path))
            {
                try
                {
                    mvm = JsonConvert
                    .DeserializeObject<MainViewModel>(File.ReadAllText(path));

                    SelectedItem = mvm.SelectedItem;

                }
                catch (Exception)
                {
                    File.Delete(path);
                }

            }
        }

        public void Sort()
        {
            itemService.Sort();
        }

        public void Save()
        {
            itemService.Save();
        }

        public void Load()
        {
            itemService.Load();
        }

        public void Search()
        {
            itemService.Search(Query);
        }
    }
}