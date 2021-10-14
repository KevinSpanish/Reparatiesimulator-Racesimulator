using System;
using System.Collections.Generic;
using System.Text;
using Model;
using static Model.Section;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }

        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants.Add(new Driver("Huey", 0, new Car(), IParticipant.TeamColors.Red));
            Competition.Participants.Add(new Driver("Dewey", 0, new Car(), IParticipant.TeamColors.Blue));
            Competition.Participants.Add(new Driver("Louie", 0, new Car(), IParticipant.TeamColors.Green));
        }

        public static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("Circuit Duckstad", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner }));
        }

        public static void NextRace()
        {
            Track nexttrack = Competition.NextTrack();
            if (nexttrack != null)
            {
                CurrentRace = new Race(nexttrack, Competition.Participants);
            }
        }
    }
}
