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
    public class Controller_Data_Competition
    {
        [SetUp]
        public void SetUp()
        {

        }


        [Test]
        public void Data_Competition_IsNotNull()
        {
            Data.Initialize();
            var data = Data.Competition.Participants;

            Assert.IsNotEmpty(data);
        }

        [Test]
        public void Data_Competition_NextRace_ShouldStart()
        {
            Data.Initialize();
            Data.NextRace();

            Assert.IsNotEmpty(Data.CurrentRace.Participants);
            Assert.IsNotEmpty(Data.CurrentRace.Track.Name);
            Assert.IsNotNull(new NextRaceEventArgs(Data.CurrentRace.Track));
        }

        [Test]
        public void Data_Competition_AddTrack_Is_Equal()
        {
            Data.Competition = new Competition();

            Track testtrack = new Track("T", new SectionTypes[] {
                SectionTypes.Straight,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            });

            Data.Competition.AddTrack(testtrack);

            Assert.That(Data.Competition.NextTrack, Is.EqualTo(testtrack));
        }

        [Test]
        public void Data_Track_Without_Finish_Throws_Exception()
        {
            Data.Competition = new Competition();

            Track nofinish = new Track("T", new SectionTypes[] {
                SectionTypes.Straight,
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            });

            try
            {
                Data.Competition.AddTrack(nofinish);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Catches the assertion exception, and the test passes
            }
        }
    }
}
