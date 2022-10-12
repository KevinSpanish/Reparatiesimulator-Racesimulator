using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.Section;

namespace ControllerTest
{
    [TestFixture]

    public class Controller_Race_MoveDriversShould
    {
        private Race _race;

        [SetUp]
        public void Setup()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.RightCorner,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
            });

            _race = new Race(track, new List<IParticipant>()
            {
                new Driver("Driver1", 0, new Car(10, 100, 10, false), IParticipant.TeamColors.Yellow),
                new Driver("Driver2", 0, new Car(10, 100, 10, false), IParticipant.TeamColors.Grey),
                new Driver("Driver3", 0, new Car(10, 100, 10, false), IParticipant.TeamColors.Blue)
            });
        }

        [Test]
        public void MoveToNextSectionWhenDistanceTravelled()
        {
            _race.Participants.RemoveAt(1);
            _race.Participants[0].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = 30;
            Assert.That(_race.MoveParticipants(), Is.EqualTo(false));
            Assert.That(_race.MoveParticipants(), Is.EqualTo(true));
        }
    }
}
