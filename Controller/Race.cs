using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;

        private Dictionary<Section, SectionData> _positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
        }

        public SectionData GetSectionData(Section section)
        {
            if (!_positions.TryGetValue(section, out SectionData value)) //Gets the value associated with the specified key.
            {
                value = new SectionData();
                _positions.Add(section, value);
            }

            return value;
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Preformance = _random.Next();
            }
        }
    }
}
