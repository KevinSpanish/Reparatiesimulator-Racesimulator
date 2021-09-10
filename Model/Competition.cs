using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Competition
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
            return null;
        }
    }
}
