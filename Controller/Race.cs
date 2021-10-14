using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _positions = new Dictionary<Section, SectionData>();
            _random = new Random(DateTime.Now.Millisecond);

            AddParticipantsPostions(Track, Participants);
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

        public void AddParticipantsPostions(Track track, List<IParticipant> participants)
        {
            int counter = 0;
            int numParticipants = participants.Count;

            Stack<Section> startgrids = new Stack<Section>();

            for (int i = 0; i < track.Sections.Count; i++)
            {
                var section = track.Sections.ElementAt(i);

                if (section.SectionType == Section.SectionTypes.StartGrid)
                {
                    startgrids.Push(section);
                }
                else if (section.SectionType == Section.SectionTypes.Finish)
                {
                    startgrids.Push(section);
                    break;
                }
            }

            while (startgrids.Count != 0)
            {
                Section section = startgrids.Pop();
                if (counter + 2 <= numParticipants)
                {
                    GetSectionData(section).Left = participants[counter];
                    GetSectionData(section).Right = participants[counter + 1];
                    counter += 2;
                }
                else if (counter + 1 <= numParticipants)
                {
                    GetSectionData(section).Left = participants[counter];
                    counter++;
                }
            }
        }
    }
}
