using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string? TrackName => Data.CurrentRace?.Track.Name;

        public DataContext()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriversChanged;
            }
        }

        public void OnDriversChanged(object s, DriversChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
