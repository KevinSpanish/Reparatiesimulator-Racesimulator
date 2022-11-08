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
                new Driver("Driver1", 0, new Car(), IParticipant.TeamColors.Red, 0),
                new Driver("Driver2", 0, new Car(), IParticipant.TeamColors.Red, 0),
                new Driver("Driver3", 0, new Car(), IParticipant.TeamColors.Red, 0),
        }, 0);
        }

        [Test]
        public void MoveParticipants_MoveToNextSectionWhenFree()
        {
            _race.MoveParticipants();

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(0));
            Assert.AreEqual(null, data.Left);
            Assert.AreEqual(null, data.Right);
        }
    }
}
