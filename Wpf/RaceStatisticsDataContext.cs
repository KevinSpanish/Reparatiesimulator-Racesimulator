using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Timer = System.Timers.Timer;

namespace Wpf
{
    public class RaceStatisticsDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Race? CurrentRace { get; set; }

        private List<IParticipant?>? _participants;
        public List<IParticipant?>? Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                OnPropertyChanged();
            }
        }

        private List<IParticipant?>? _finishedParticipants;
        public List<IParticipant?>? FinishedParticipants
        {
            get => _finishedParticipants;
            set
            {
                _finishedParticipants = value;
                OnPropertyChanged();
            }
        }

        public string? TrackName => Data.CurrentRace?.Track.Name;


        public RaceStatisticsDataContext() : this(Data.CurrentRace, Data.Competition?.Participants, Data.CurrentRace?.FinishedParticipants)
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        public RaceStatisticsDataContext(Race? currentRace, List<IParticipant>? participants, List<IParticipant>? finishedparticipants)
        {
            CurrentRace = currentRace;
            Participants = participants;
            FinishedParticipants = finishedparticipants;
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            if (CurrentRace?.Participants != null)
            {
                Participants = CurrentRace.Participants.ToList();

                if (CurrentRace.FinishedParticipants != null)
                {
                    FinishedParticipants = CurrentRace.FinishedParticipants.ToList();
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        private void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
