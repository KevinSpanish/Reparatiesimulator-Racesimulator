using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf
{
    public class CompetitionStatisticsDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<IParticipant?>? _participantrankings;
        public List<IParticipant?>? ParticipantRankings
        {
            get => _participantrankings;
            set
            {
                _participantrankings = value;
                OnPropertyChanged();
            }
        }
        
        private Track? _nexttrack;
        public Track? NextTrack {
            get => _nexttrack;
            set
            {
                _nexttrack = value;
                OnPropertyChanged();
            }
        }

        public CompetitionStatisticsDataContext() : this(Data.Competition.Participants)
        {
            //TODO: Change when new race is invoked. Now the window needs to be reopened.

            ParticipantRankings = Data.Competition.Participants;
            NextTrack = Data.Competition.Tracks.Peek();
            Data.CurrentRace.NextRace += OnRaceFinished;
        }

        public CompetitionStatisticsDataContext(List<IParticipant>? participantrankings)
        {
            ParticipantRankings = participantrankings;
        }

        public void OnRaceFinished(object sender, NextRaceEventArgs e)
        {
            NextTrack = Data.Competition.Tracks.Peek();
            ParticipantRankings = SortParticipants(Data.Competition.Participants);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public List<IParticipant> SortParticipants(List<IParticipant> participants)
        {
            return participants.OrderBy(p => p.Points).ToList();
        }

        private void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
