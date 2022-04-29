using Library.ListManagement.Standard.utilities;
using ListManagement.models;
using ListManagement.services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UWPListManagement.Dialogs;
using UWPListManagement.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPListManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string persistencePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SaveData.json";

        public MainPage()
        {
            this.InitializeComponent();

            

      
            DataContext = new MainViewModel();
            
        }

        private async void AddToDoClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ToDoDialog((DataContext as MainViewModel).Items);
            await dialog.ShowAsync();
        }

        private async void AddAppointmentClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AppointmentDialog((DataContext as MainViewModel).Items);
            await dialog.ShowAsync();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Load();
        }

        private async void EditItemClick(object sender, RoutedEventArgs e)
        {
            var Type = (DataContext as MainViewModel).SelectedItem.GetType();
            if (Type == typeof(ToDo)){
                var dialog = new ToDoDialog((DataContext as MainViewModel).Items, (DataContext as MainViewModel).SelectedItem);
                await dialog.ShowAsync();
            }
            else if (Type == typeof(Appointment)) {
                var dialog = new AppointmentDialog((DataContext as MainViewModel).Items, (DataContext as MainViewModel).SelectedItem);
                await dialog.ShowAsync();

            }
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Remove((DataContext as MainViewModel).SelectedItem);
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Sort();
           
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Search();
        }
    }
}