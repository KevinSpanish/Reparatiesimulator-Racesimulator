using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }

        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Track NextTrack()
        {
            if (Tracks.Count == 0)
            {
                return null;
            }
            return Tracks.Dequeue();
        }

        public void AddTrack(Track track)
        {
            bool hasFinish = false;

            foreach (Section section in track.Sections)
            {
                if (section.SectionType == Section.SectionTypes.Finish)
                {
                    hasFinish = true;
                }
            }

            if (!hasFinish)
            {
                throw new ArgumentException($"Track {track.Name} has no finish section");
            }

            Tracks.Enqueue(track);
        }
    }
}
