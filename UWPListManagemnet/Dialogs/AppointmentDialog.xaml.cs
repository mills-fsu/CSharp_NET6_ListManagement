using ListManagement.models;
using ListManagement.services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPListManagement.Dialogs
{
    public sealed partial class AppointmentDialog : ContentDialog
    {
        private ObservableCollection<Item> _appointmentCollection;
        public AppointmentDialog(ObservableCollection<Item> list)
        {
            this.InitializeComponent();
            _appointmentCollection = list;

            DataContext = new Appointment();
        }

        public AppointmentDialog(ObservableCollection<Item> list, Item item)
        {
            this.InitializeComponent();
            _appointmentCollection = list;
            DataContext = item;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var item = DataContext as Appointment;
            if (_appointmentCollection.Any(i => i.Id == item.Id))
            {
                var itemToUpdate = _appointmentCollection.FirstOrDefault(i => i.Id == item.Id);
                var index = _appointmentCollection.IndexOf(itemToUpdate);
                _appointmentCollection.RemoveAt(index);
                _appointmentCollection.Insert(index, item);
            }
            else
            {
                ItemService.Current.Add(DataContext as Appointment);
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}